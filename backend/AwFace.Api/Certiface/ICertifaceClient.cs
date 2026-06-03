namespace AwFace.Api.Certiface;

public interface ICertifaceClient
{
    Task<CertifaceHttpResponse> CheckAppKeyAsync(
        string? queryString,
        CancellationToken cancellationToken);

    Task<CertifaceHttpResponse> StartLiveness2dChallengeAsync(
        string requestBody,
        string? contentType,
        CancellationToken cancellationToken);

    Task<CertifaceHttpResponse> ValidateLiveness2dCaptchaAsync(
        string requestBody,
        string? contentType,
        CancellationToken cancellationToken);

    Task<CertifaceHttpResponse> InitializeLiveness3dAsync(
        string requestBody,
        string? contentType,
        CancellationToken cancellationToken);

    Task<CertifaceHttpResponse> CreateLiveness3dSessionTokenAsync(
        string requestBody,
        string? contentType,
        CancellationToken cancellationToken);

    Task<CertifaceHttpResponse> ProcessFaceTecV10SessionRequestAsync(
        string requestBody,
        string? contentType,
        CancellationToken cancellationToken);

    Task<CertifaceHttpResponse> ValidateLiveness3dAsync(
        string requestBody,
        string? contentType,
        CancellationToken cancellationToken);

    Task<CertifaceHttpResponse> SendDocumentAsync(
        string requestBody,
        string? contentType,
        CancellationToken cancellationToken);
}
