namespace AwFace.Api.Domain;

public class Journey
{
    public Guid Id { get; set; }
    public Guid TenantId { get; set; }
    public string Cpf { get; set; } = string.Empty;
    public string Nome { get; set; } = string.Empty;
    public string Nascimento { get; set; } = string.Empty;
    public string IdExternoCliente { get; set; } = string.Empty;
    public string ValidationStatus { get; set; } = "Pending"; // Pending, Valid, Invalid
    public string SessionToken { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
