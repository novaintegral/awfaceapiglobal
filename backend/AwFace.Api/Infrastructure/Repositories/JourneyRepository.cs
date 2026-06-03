using AwFace.Api.Domain;
using Dapper;

namespace AwFace.Api.Infrastructure.Repositories;

public class JourneyRepository(DbConnectionFactory connectionFactory)
{
    public async Task<Journey?> GetByIdAsync(Guid id)
    {
        using var connection = connectionFactory.CreateConnection();
        const string sql = "SELECT * FROM Journeys WHERE Id = @Id";
        return await connection.QuerySingleOrDefaultAsync<Journey>(sql, new { Id = id });
    }

    public async Task<IEnumerable<Journey>> GetByCpfAsync(string cpf)
    {
        using var connection = connectionFactory.CreateConnection();
        const string sql = "SELECT * FROM Journeys WHERE Cpf = @Cpf";
        return await connection.QueryAsync<Journey>(sql, new { Cpf = cpf });
    }

    public async Task CreateAsync(Journey journey)
    {
        using var connection = connectionFactory.CreateConnection();
        const string sql = @"
            INSERT INTO Journeys (Id, TenantId, Cpf, Nome, Nascimento, IdExternoCliente, ValidationStatus, SessionToken, CreatedAt, UpdatedAt)
            VALUES (@Id, @TenantId, @Cpf, @Nome, @Nascimento, @IdExternoCliente, @ValidationStatus, @SessionToken, @CreatedAt, @UpdatedAt)";
        await connection.ExecuteAsync(sql, journey);
    }

    public async Task UpdateStatusAsync(Guid id, string status)
    {
        using var connection = connectionFactory.CreateConnection();
        const string sql = @"
            UPDATE Journeys
            SET ValidationStatus = @Status, UpdatedAt = @UpdatedAt
            WHERE Id = @Id";
        await connection.ExecuteAsync(sql, new { Id = id, Status = status, UpdatedAt = DateTime.UtcNow });
    }
}
