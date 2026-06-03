using System.Data;
using Npgsql;
using Microsoft.Extensions.Configuration;

namespace AwFace.Api.Infrastructure;

public class DbConnectionFactory(IConfiguration configuration)
{
    private readonly string _connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

    public IDbConnection CreateConnection()
    {
        return new NpgsqlConnection(_connectionString);
    }
}
