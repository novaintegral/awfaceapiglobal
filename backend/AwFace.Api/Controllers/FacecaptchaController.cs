using AwFace.Api.Certiface;
using Microsoft.AspNetCore.Mvc;

namespace AwFace.Api.Controllers;

[ApiController]
[Route("api/facecaptcha/service/captcha")]
public sealed class FacecaptchaController(ICertifaceProxy certifaceProxy) : ControllerBase
{
    private const string CertifaceBasePath = "/facecaptcha/service/captcha";

    [HttpGet("checkauth")]
    public Task<IResult> CheckAuth(CancellationToken cancellationToken) =>
        ForwardAsync($"{CertifaceBasePath}/checkauth", cancellationToken);

    [HttpPost("challenge")]
    public Task<IResult> Challenge(CancellationToken cancellationToken) =>
        ForwardAsync($"{CertifaceBasePath}/challenge", cancellationToken);

    [HttpPost]
    public Task<IResult> Captcha(CancellationToken cancellationToken) =>
        ForwardAsync(CertifaceBasePath, cancellationToken);

    [HttpPost("3d/initialize")]
    public Task<IResult> Initialize3d(CancellationToken cancellationToken) =>
        ForwardAsync($"{CertifaceBasePath}/3d/initialize", cancellationToken);

    [HttpPost("3d/session-token")]
    public Task<IResult> SessionToken(CancellationToken cancellationToken) =>
        ForwardAsync($"{CertifaceBasePath}/3d/session-token", cancellationToken);

    [HttpPost("3d/process-request")]
    public Task<IResult> ProcessRequest(CancellationToken cancellationToken) =>
        ForwardAsync($"{CertifaceBasePath}/3d/process-request", cancellationToken);

    [HttpPost("document")]
    public Task<IResult> Document(CancellationToken cancellationToken) =>
        ForwardAsync($"{CertifaceBasePath}/document", cancellationToken);

    [HttpPost("3d/liveness")]
    public Task<IResult> Liveness3d(CancellationToken cancellationToken) =>
        ForwardAsync($"{CertifaceBasePath}/3d/liveness", cancellationToken);

    private Task<IResult> ForwardAsync(string certifacePath, CancellationToken cancellationToken) =>
        certifaceProxy.ForwardAsync(Request, certifacePath, cancellationToken);
}
