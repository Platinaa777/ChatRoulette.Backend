using Microsoft.Extensions.Options;
using Npgsql;
using ProfileService.Infrastructure.Configuration;
using ProfileService.Infrastructure.PersistenceAbstractions;

namespace ProfileService.Infrastructure.Repos.Common;

public class NpgsqlConnectionFactory : IDbConnectionFactory<NpgsqlConnection>
{
    private readonly string _connectionString;
    private NpgsqlConnection? _connection;

    public NpgsqlConnectionFactory(IOptions<DatabaseOptions> connectionString)
    {
        _connectionString = connectionString.Value.ConnectionString;
        Console.WriteLine(_connectionString);
    }
    
    public async Task<NpgsqlConnection> CreateConnectionAsync(CancellationToken token)
    {
        if (_connection != null) return _connection;

        _connection = new NpgsqlConnection(_connectionString);
        await _connection.OpenAsync(token);
        return _connection;    
    }

    public void Dispose()
    {
        _connection?.Dispose();
    }
}