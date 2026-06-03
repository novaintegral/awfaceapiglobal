namespace AwFace.Api.Domain;

public class AuditLog
{
    public Guid Id { get; set; }
    public Guid JourneyId { get; set; }
    public string Action { get; set; } = string.Empty; // e.g., "JourneyCreated", "WebhookAttempt", "WebhookFailed"
    public string Details { get; set; } = string.Empty;
    public bool DeliveryFailed { get; set; } // Flag for manual intervention
    public DateTime CreatedAt { get; set; }
}
