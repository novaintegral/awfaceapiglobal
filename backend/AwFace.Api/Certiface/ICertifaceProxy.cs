namespace AwFace.Api.Certiface;

public interface ICertifaceProxy
{
    Task<IResult> ForwardAsync(HttpRequest request, string certifacePath, CancellationToken cancellationToken);
}
