using AwFace.Api.Certiface;
using Microsoft.AspNetCore.Mvc;

namespace AwFace.Api.Controllers;

[ApiController]
[Route("api/liveness")]
public sealed class LivenessController(ICertifaceClient certifaceClient) : ControllerBase
{
    [HttpPost("2d/challenge")]
    public Task<IResult> StartLiveness2dChallenge(CancellationToken cancellationToken) =>
        ForwardBodyAsync(certifaceClient.StartLiveness2dChallengeAsync, cancellationToken);

    [HttpPost("2d/captcha")]
    public Task<IResult> ValidateLiveness2dCaptcha(CancellationToken cancellationToken) =>
        ForwardBodyAsync(certifaceClient.ValidateLiveness2dCaptchaAsync, cancellationToken);

    [HttpPost("3d/initialize")]
    public Task<IResult> InitializeLiveness3d(CancellationToken cancellationToken) =>
        ForwardBodyAsync(certifaceClient.InitializeLiveness3dAsync, cancellationToken);

    [HttpPost("3d/session-token")]
    public Task<IResult> CreateLiveness3dSessionToken(CancellationToken cancellationToken) =>
        ForwardBodyAsync(certifaceClient.CreateLiveness3dSessionTokenAsync, cancellationToken);

    [HttpPost("facetec-v10/session-request")]
    public Task<IResult> ProcessFaceTecV10SessionRequest(CancellationToken cancellationToken) =>
        ForwardBodyAsync(certifaceClient.ProcessFaceTecV10SessionRequestAsync, cancellationToken);

    [HttpPost("3d/validation")]
    public Task<IResult> ValidateLiveness3d(CancellationToken cancellationToken) =>
        ForwardBodyAsync(certifaceClient.ValidateLiveness3dAsync, cancellationToken);

    private async Task<IResult> ForwardBodyAsync(
        Func<string, string?, CancellationToken, Task<CertifaceHttpResponse>> operation,
        CancellationToken cancellationToken)
    {
        using var reader = new StreamReader(Request.Body, leaveOpen: true);
        var body = await reader.ReadToEndAsync(cancellationToken);

        var response = await operation(
            body,
            Request.ContentType,
            cancellationToken);

        return new ProxyResult(response.Body, response.ContentType, response.StatusCode);
    }
}
