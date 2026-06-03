using AwFace.Api.Certiface;
using Microsoft.AspNetCore.Mvc;

namespace AwFace.Api.Controllers;

[ApiController]
[Route("api/documents")]
public sealed class DocumentsController(ICertifaceClient certifaceClient) : ControllerBase
{
    [HttpPost]
    public Task<IResult> SendDocument(CancellationToken cancellationToken) =>
        SendToCertiface(cancellationToken);

    [HttpPost("digital-cnh")]
    public Task<IResult> SendDigitalCnh(CancellationToken cancellationToken) =>
        SendToCertiface(cancellationToken);

    private async Task<IResult> SendToCertiface(CancellationToken cancellationToken)
    {
        using var reader = new StreamReader(Request.Body, leaveOpen: true);
        var body = await reader.ReadToEndAsync(cancellationToken);

        var response = await certifaceClient.SendDocumentAsync(
            body,
            Request.ContentType,
            cancellationToken);

        return new ProxyResult(response.Body, response.ContentType, response.StatusCode);
    }
}
