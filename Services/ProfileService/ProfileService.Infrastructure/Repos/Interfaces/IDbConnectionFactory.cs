namespace ProfileService.Infrastructure.Repos.Interfaces;

public interface IDbConnectionFactory<TConnection> : IDisposable
{
    Task<TConnection> CreateConnection(CancellationToken token);
}