namespace AwFace.Api.Certiface;

public sealed class ProxyResult(byte[] body, string? contentType, int statusCode) : IResult
{
    public async Task ExecuteAsync(HttpContext httpContext)
    {
        httpContext.Response.StatusCode = statusCode;

        if (!string.IsNullOrWhiteSpace(contentType))
        {
            httpContext.Response.ContentType = contentType;
        }

        await httpContext.Response.Body.WriteAsync(body);
    }
}
