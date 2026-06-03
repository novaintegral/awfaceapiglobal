namespace AwFace.Api.Certiface;

public sealed record CertifaceHttpResponse(
    byte[] Body,
    string? ContentType,
    int StatusCode);
