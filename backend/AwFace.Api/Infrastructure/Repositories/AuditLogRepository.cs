using AwFace.Api.Domain;
using Dapper;

namespace AwFace.Api.Infrastructure.Repositories;

public class AuditLogRepository(DbConnectionFactory connectionFactory)
{
    public async Task CreateAsync(AuditLog log)
    {
        using var connection = connectionFactory.CreateConnection();
        const string sql = @"
            INSERT INTO AuditLogs (Id, JourneyId, Action, Details, DeliveryFailed, CreatedAt)
            VALUES (@Id, @JourneyId, @Action, @Details, @DeliveryFailed, @CreatedAt)";
        await connection.ExecuteAsync(sql, log);
    }
}
