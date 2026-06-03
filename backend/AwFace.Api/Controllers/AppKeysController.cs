using AwFace.Api.Certiface;
using Microsoft.AspNetCore.Mvc;

namespace AwFace.Api.Controllers;

[ApiController]
[Route("api/appkeys")]
public sealed class AppKeysController(ICertifaceClient certifaceClient) : ControllerBase
{
    [HttpGet("checkauth")]
    public async Task<IResult> CheckAuth(CancellationToken cancellationToken)
    {
        var response = await certifaceClient.CheckAppKeyAsync(
            Request.QueryString.Value,
            cancellationToken);

        return new ProxyResult(response.Body, response.ContentType, response.StatusCode);
    }
}
