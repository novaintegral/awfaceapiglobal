using System.Text;
using System.Text.Json;
using AwFace.Api.Domain;
using AwFace.Api.Infrastructure.Repositories;
using Microsoft.Extensions.Logging;

namespace AwFace.Api.Infrastructure.Services;

public class WebhookService(
    HttpClient httpClient,
    AuditLogRepository auditLogRepository,
    ILogger<WebhookService> logger)
{
    private const int MaxRetries = 3;

    public async Task SendWebhookAsync(string url, object payload, Guid journeyId)
    {
        var attempt = 0;
        var success = false;
        var jsonPayload = JsonSerializer.Serialize(payload);
        var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

        while (attempt < MaxRetries && !success)
        {
            attempt++;
            try
            {
                var response = await httpClient.PostAsync(url, content);
                if (response.IsSuccessStatusCode)
                {
                    success = true;
                    await auditLogRepository.CreateAsync(new AuditLog
                    {
                        Id = Guid.NewGuid(),
                        JourneyId = journeyId,
                        Action = "WebhookSuccess",
                        Details = $"Attempt {attempt} successful.",
                        DeliveryFailed = false,
                        CreatedAt = DateTime.UtcNow
                    });
                }
                else
                {
                    logger.LogWarning("Webhook failed on attempt {Attempt} with status {StatusCode}", attempt, response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Exception on webhook attempt {Attempt}", attempt);
            }

            if (!success && attempt < MaxRetries)
            {
                await Task.Delay(TimeSpan.FromSeconds(Math.Pow(2, attempt))); // Exponential backoff
            }
        }

        if (!success)
        {
            logger.LogError("Webhook definitively failed after {MaxRetries} attempts for journey {JourneyId}", MaxRetries, journeyId);
            await auditLogRepository.CreateAsync(new AuditLog
            {
                Id = Guid.NewGuid(),
                JourneyId = journeyId,
                Action = "WebhookFailed",
                Details = $"Definitively failed after {MaxRetries} attempts.",
                DeliveryFailed = true,
                CreatedAt = DateTime.UtcNow
            });
        }
    }
}
