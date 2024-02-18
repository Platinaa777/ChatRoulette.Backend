namespace ProfileService.Domain.SeedWork;

public interface IUnitOfWork : IDisposable
{
    ValueTask StartTransaction(CancellationToken token);
    Task SaveChangesAsync(CancellationToken token);
}