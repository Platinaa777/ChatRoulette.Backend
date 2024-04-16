namespace ProfileService.Infrastructure.PersistenceAbstractions;

public interface IDbConnectionFactory<TConnection> : IDisposable
{
    Task<TConnection> CreateConnectionAsync(CancellationToken token);
}