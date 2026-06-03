using AwFace.Api.Domain;
using AwFace.Api.Infrastructure.Repositories;
using AwFace.Api.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace AwFace.Api.Controllers;

[ApiController]
[Route("api/journeys")]
public class JourneyController(
    TenantRepository tenantRepository,
    JourneyRepository journeyRepository,
    WebhookService webhookService) : ControllerBase
{
    [HttpPost("initialize")]
    public async Task<IActionResult> Initialize([FromQuery] string token_tenant, [FromQuery] string cpf, [FromQuery] string nome, [FromQuery] string nascimento, [FromQuery] string idExternoCliente)
    {
        var tenant = await tenantRepository.GetByTokenAsync(token_tenant);
        if (tenant == null)
        {
            return Unauthorized("Invalid tenant token.");
        }

        var journey = new Journey
        {
            Id = Guid.NewGuid(),
            TenantId = tenant.Id,
            Cpf = cpf,
            Nome = nome,
            Nascimento = nascimento,
            IdExternoCliente = idExternoCliente,
            ValidationStatus = "Pending",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await journeyRepository.CreateAsync(journey);

        return Ok(new
        {
            JourneyId = journey.Id,
            TenantTheme = new
            {
                tenant.LogoBase64,
                tenant.TermsOfUseLink,
                tenant.PrivacyPolicyLink
            }
        });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var journey = await journeyRepository.GetByIdAsync(id);
        if (journey == null)
        {
            return NotFound();
        }
        return Ok(journey);
    }

    [HttpGet("by-cpf/{cpf}")]
    public async Task<IActionResult> GetByCpf(string cpf)
    {
        var journeys = await journeyRepository.GetByCpfAsync(cpf);
        return Ok(journeys);
    }

    // Example endpoint to simulate processing the result and sending the webhook
    [HttpPost("{id}/complete")]
    public async Task<IActionResult> Complete(Guid id, [FromBody] CompleteJourneyDto dto)
    {
        var journey = await journeyRepository.GetByIdAsync(id);
        if (journey == null)
        {
            return NotFound();
        }

        var tenant = await tenantRepository.GetByIdAsync(journey.TenantId);
        if (tenant == null)
        {
            return NotFound("Tenant not found.");
        }

        await journeyRepository.UpdateStatusAsync(id, dto.Status);

        // Fire and forget webhook
        _ = webhookService.SendWebhookAsync(tenant.TargetWebhookUrl, new {
            JourneyId = journey.Id,
            Status = dto.Status,
            Cpf = journey.Cpf
        }, journey.Id);

        return Ok();
    }
}

public class CompleteJourneyDto
{
    public string Status { get; set; } = string.Empty;
}
