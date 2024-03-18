using AuthService.DataContext.Database;
using AuthService.Domain.Models.TokenAggregate;
using AuthService.Domain.Models.TokenAggregate.Repos;
using AuthService.Domain.Models.TokenAggregate.ValueObjects.Token;
using AuthService.Domain.Models.UserAggregate.Repos;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Infrastructure.Repos;

public class RefreshTokenRepository : ITokenRepository
{
    private readonly UserDb _context;

    public RefreshTokenRepository(UserDb context)
    {
        _context = context;
    }   
    
    public async Task<bool> AddRefreshToken(RefreshToken token)
    {
        await _context.RefreshTokens.AddAsync(token);

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdateRefreshToken(RefreshToken refreshToken)
    {
        var result = await _context.RefreshTokens.Where(token => token.Token == refreshToken.Token)
            .ExecuteUpdateAsync(entity => entity
                .SetProperty(dbToken => dbToken.IsUsed, refreshToken.IsUsed));
        return result == 1;
    }

    public async Task<RefreshToken?> GetRefreshTokenByValue(Token token)
    {
        return await _context.RefreshTokens.FirstOrDefaultAsync(x => x.Token == token);
    }

    public async Task<RefreshToken?> FindValidRefreshTokenByUserId(UserId userId)
    {
        return await _context.RefreshTokens.FirstOrDefaultAsync(t => t.UserId == userId && !t.IsUsed);
    }
}