using System.ComponentModel.DataAnnotations;

namespace AwFace.Api.Options;

public sealed class CertifaceOptions
{
    public const string SectionName = "Certiface";

    [Required]
    public Uri BaseUrl { get; init; } = new("https://hml.certiface.com.br");

    public string? Login { get; init; }

    public string? Password { get; init; }

    [Range(1, 300)]
    public int TimeoutSeconds { get; init; } = 60;
}
