using System.Net.Http.Headers;
using System.Text;
using AwFace.Api.Options;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;

namespace AwFace.Api.Certiface;

public sealed class CertifaceProxy(HttpClient httpClient, IOptions<CertifaceOptions> options) : ICertifaceProxy
{
    private static readonly HashSet<string> HopByHopHeaders = new(StringComparer.OrdinalIgnoreCase)
    {
        "Connection",
        "Keep-Alive",
        "Proxy-Authenticate",
        "Proxy-Authorization",
        "TE",
        "Trailer",
        "Transfer-Encoding",
        "Upgrade",
        "Host",
        "Accept-Encoding"
    };

    private readonly CertifaceOptions options = options.Value;

    public async Task<IResult> ForwardAsync(
        HttpRequest request,
        string certifacePath,
        CancellationToken cancellationToken)
    {
        using var outboundRequest = await CreateOutboundRequestAsync(request, certifacePath, cancellationToken);
        using var outboundResponse = await httpClient.SendAsync(
            outboundRequest,
            HttpCompletionOption.ResponseHeadersRead,
            cancellationToken);

        var responseBody = await outboundResponse.Content.ReadAsByteArrayAsync(cancellationToken);
        var contentType = outboundResponse.Content.Headers.ContentType?.ToString();

        return new ProxyResult(responseBody, contentType, (int)outboundResponse.StatusCode);
    }

    private async Task<HttpRequestMessage> CreateOutboundRequestAsync(
        HttpRequest request,
        string certifacePath,
        CancellationToken cancellationToken)
    {
        var target = BuildTargetUri(request, certifacePath);
        var outboundRequest = new HttpRequestMessage(new HttpMethod(request.Method), target);

        foreach (var header in request.Headers)
        {
            if (HopByHopHeaders.Contains(header.Key))
            {
                continue;
            }

            outboundRequest.Headers.TryAddWithoutValidation(header.Key, header.Value.AsEnumerable());
        }

        ApplyBasicAuthIfConfigured(outboundRequest);

        if (HttpMethods.IsPost(request.Method) || HttpMethods.IsPut(request.Method) || HttpMethods.IsPatch(request.Method))
        {
            using var reader = new StreamReader(request.Body, Encoding.UTF8, leaveOpen: true);
            var body = await reader.ReadToEndAsync(cancellationToken);

            outboundRequest.Content = new StringContent(body, Encoding.UTF8);
            if (!string.IsNullOrWhiteSpace(request.ContentType))
            {
                outboundRequest.Content.Headers.ContentType = MediaTypeHeaderValue.Parse(request.ContentType);
            }
        }

        return outboundRequest;
    }

    private static Uri BuildTargetUri(HttpRequest request, string certifacePath)
    {
        var queryString = request.QueryString.HasValue ? request.QueryString.Value : string.Empty;
        return new Uri($"{certifacePath}{queryString}", UriKind.Relative);
    }

    private void ApplyBasicAuthIfConfigured(HttpRequestMessage outboundRequest)
    {
        if (string.IsNullOrWhiteSpace(options.Login) || string.IsNullOrWhiteSpace(options.Password))
        {
            return;
        }

        var credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{options.Login}:{options.Password}"));
        outboundRequest.Headers.Authorization = new AuthenticationHeaderValue("Basic", credentials);
    }
}
