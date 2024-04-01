using AuthService.DataContext.Database;
using DomainDriverDesignAbstractions;

namespace AuthService.Infrastructure.Repos;

public class UnitOfWork : IUnitOfWork
{
    private readonly UserDb _context;

    public UnitOfWork(UserDb context)
    {
        _context = context;
    }

    public ValueTask StartTransaction(CancellationToken token = default)
    {
        return ValueTask.CompletedTask;
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }

    public void Dispose() { }
}