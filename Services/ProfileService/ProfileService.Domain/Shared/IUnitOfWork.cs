namespace ProfileService.Domain.Shared;

public interface IUnitOfWork : IDisposable
{
    ValueTask StartTransaction(CancellationToken token);
    Task SaveChangesAsync(CancellationToken token);
}