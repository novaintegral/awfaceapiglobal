using System.Net.Http.Headers;
using System.Text;
using AwFace.Api.Options;
using Microsoft.Extensions.Options;

namespace AwFace.Api.Certiface;

public sealed class CertifaceClient(HttpClient httpClient, IOptions<CertifaceOptions> options) : ICertifaceClient
{
    private const string CaptchaBasePath = "/facecaptcha/service/captcha";
    private const string CheckAppKeyPath = $"{CaptchaBasePath}/checkauth";
    private const string ChallengePath = $"{CaptchaBasePath}/challenge";
    private const string Initialize3dPath = $"{CaptchaBasePath}/3d/initialize";
    private const string SessionToken3dPath = $"{CaptchaBasePath}/3d/session-token";
    private const string FaceTecV10ProcessRequestPath = "/facecaptcha/service/captcha/3d/process-request";
    private const string Liveness3dPath = $"{CaptchaBasePath}/3d/liveness";
    private const string DocumentPath = $"{CaptchaBasePath}/document";

    private readonly CertifaceOptions options = options.Value;

    public Task<CertifaceHttpResponse> CheckAppKeyAsync(
        string? queryString,
        CancellationToken cancellationToken) =>
        SendAsync(HttpMethod.Get, $"{CheckAppKeyPath}{queryString}", null, null, cancellationToken);

    public Task<CertifaceHttpResponse> StartLiveness2dChallengeAsync(
        string requestBody,
        string? contentType,
        CancellationToken cancellationToken) =>
        PostAsync(ChallengePath, requestBody, contentType, cancellationToken);

    public Task<CertifaceHttpResponse> ValidateLiveness2dCaptchaAsync(
        string requestBody,
        string? contentType,
        CancellationToken cancellationToken) =>
        PostAsync(CaptchaBasePath, requestBody, contentType, cancellationToken);

    public Task<CertifaceHttpResponse> InitializeLiveness3dAsync(
        string requestBody,
        string? contentType,
        CancellationToken cancellationToken) =>
        PostAsync(Initialize3dPath, requestBody, contentType, cancellationToken);

    public Task<CertifaceHttpResponse> CreateLiveness3dSessionTokenAsync(
        string requestBody,
        string? contentType,
        CancellationToken cancellationToken) =>
        PostAsync(SessionToken3dPath, requestBody, contentType, cancellationToken);

    public Task<CertifaceHttpResponse> ProcessFaceTecV10SessionRequestAsync(
        string requestBody,
        string? contentType,
        CancellationToken cancellationToken) =>
        PostAsync(FaceTecV10ProcessRequestPath, requestBody, contentType, cancellationToken);

    public Task<CertifaceHttpResponse> ValidateLiveness3dAsync(
        string requestBody,
        string? contentType,
        CancellationToken cancellationToken) =>
        PostAsync(Liveness3dPath, requestBody, contentType, cancellationToken);

    public Task<CertifaceHttpResponse> SendDocumentAsync(
        string requestBody,
        string? contentType,
        CancellationToken cancellationToken) =>
        PostAsync(DocumentPath, requestBody, contentType, cancellationToken);

    private async Task<CertifaceHttpResponse> PostAsync(
        string path,
        string requestBody,
        string? contentType,
        CancellationToken cancellationToken) =>
        await SendAsync(HttpMethod.Post, path, requestBody, contentType, cancellationToken);

    private async Task<CertifaceHttpResponse> SendAsync(
        HttpMethod method,
        string path,
        string? requestBody,
        string? contentType,
        CancellationToken cancellationToken)
    {
        using var request = new HttpRequestMessage(method, new Uri(path, UriKind.Relative));

        if (requestBody is not null)
        {
            request.Content = new StringContent(requestBody, Encoding.UTF8);
            request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse(
                string.IsNullOrWhiteSpace(contentType) ? "application/json" : contentType);
        }

        ApplyBasicAuthIfConfigured(request);

        using var response = await httpClient.SendAsync(
            request,
            HttpCompletionOption.ResponseHeadersRead,
            cancellationToken);

        return new CertifaceHttpResponse(
            await response.Content.ReadAsByteArrayAsync(cancellationToken),
            response.Content.Headers.ContentType?.ToString(),
            (int)response.StatusCode);
    }

    private void ApplyBasicAuthIfConfigured(HttpRequestMessage request)
    {
        if (string.IsNullOrWhiteSpace(options.Login) || string.IsNullOrWhiteSpace(options.Password))
        {
            return;
        }

        var credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{options.Login}:{options.Password}"));
        request.Headers.Authorization = new AuthenticationHeaderValue("Basic", credentials);
    }
}
