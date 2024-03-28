using AuthService.DataContext.Database;
using AuthService.Domain.Shared;

namespace AuthService.Infrastructure.Repos;

public class UnitOfWork : IUnitOfWork
{
    private readonly UserDb _context;

    public UnitOfWork(UserDb context)
    {
        _context = context;
    }
    
    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }
}