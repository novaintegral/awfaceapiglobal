namespace AwFace.Api.Domain;

public class Tenant
{
    public Guid Id { get; set; }
    public string Token { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string FacetecCredential { get; set; } = string.Empty;
    public string FacetecAppKey { get; set; } = string.Empty;
    public string TargetWebhookUrl { get; set; } = string.Empty;
    public string TermsOfUseLink { get; set; } = string.Empty;
    public string PrivacyPolicyLink { get; set; } = string.Empty;
    public string LogoBase64 { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
