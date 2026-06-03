using AwFace.Api.Domain;
using AwFace.Api.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace AwFace.Api.Controllers;

[ApiController]
[Route("api/tenants")]
public class TenantController(TenantRepository tenantRepository) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] TenantDto dto)
    {
        var tenant = new Tenant
        {
            Id = Guid.NewGuid(),
            Token = Guid.NewGuid().ToString("N"),
            Name = dto.Name,
            FacetecCredential = dto.FacetecCredential,
            FacetecAppKey = dto.FacetecAppKey,
            TargetWebhookUrl = dto.TargetWebhookUrl,
            TermsOfUseLink = dto.TermsOfUseLink,
            PrivacyPolicyLink = dto.PrivacyPolicyLink,
            LogoBase64 = dto.LogoBase64,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await tenantRepository.CreateAsync(tenant);

        return CreatedAtAction(nameof(Get), new { id = tenant.Id }, tenant);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var tenant = await tenantRepository.GetByIdAsync(id);
        if (tenant == null)
        {
            return NotFound();
        }
        return Ok(tenant);
    }
}

public class TenantDto
{
    public string Name { get; set; } = string.Empty;
    public string FacetecCredential { get; set; } = string.Empty;
    public string FacetecAppKey { get; set; } = string.Empty;
    public string TargetWebhookUrl { get; set; } = string.Empty;
    public string TermsOfUseLink { get; set; } = string.Empty;
    public string PrivacyPolicyLink { get; set; } = string.Empty;
    public string LogoBase64 { get; set; } = string.Empty;
}
