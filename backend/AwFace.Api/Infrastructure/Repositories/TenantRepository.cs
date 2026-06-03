using AwFace.Api.Domain;
using Dapper;

namespace AwFace.Api.Infrastructure.Repositories;

public class TenantRepository(DbConnectionFactory connectionFactory)
{
    public async Task<Tenant?> GetByIdAsync(Guid id)
    {
        using var connection = connectionFactory.CreateConnection();
        const string sql = "SELECT * FROM Tenants WHERE Id = @Id";
        return await connection.QuerySingleOrDefaultAsync<Tenant>(sql, new { Id = id });
    }

    public async Task<Tenant?> GetByTokenAsync(string token)
    {
        using var connection = connectionFactory.CreateConnection();
        const string sql = "SELECT * FROM Tenants WHERE Token = @Token";
        return await connection.QuerySingleOrDefaultAsync<Tenant>(sql, new { Token = token });
    }

    public async Task CreateAsync(Tenant tenant)
    {
        using var connection = connectionFactory.CreateConnection();
        const string sql = @"
            INSERT INTO Tenants (Id, Token, Name, FacetecCredential, FacetecAppKey, TargetWebhookUrl, TermsOfUseLink, PrivacyPolicyLink, LogoBase64, CreatedAt, UpdatedAt)
            VALUES (@Id, @Token, @Name, @FacetecCredential, @FacetecAppKey, @TargetWebhookUrl, @TermsOfUseLink, @PrivacyPolicyLink, @LogoBase64, @CreatedAt, @UpdatedAt)";
        await connection.ExecuteAsync(sql, tenant);
    }
}
