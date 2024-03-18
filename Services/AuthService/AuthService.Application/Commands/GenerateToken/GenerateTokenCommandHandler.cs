using System.Security.Claims;
using AuthService.Application.Models;
using AuthService.Application.Security;
using AuthService.Application.Utils;
using AuthService.Domain.Models.TokenAggregate;
using AuthService.Domain.Models.TokenAggregate.Repos;
using AuthService.Domain.Models.TokenAggregate.ValueObjects.Token;
using AuthService.Domain.Models.UserAggregate.Repos;
using MediatR;

namespace AuthService.Application.Commands.GenerateToken;

public class GenerateTokenCommandHandler : IRequestHandler<GenerateTokenCommand, AuthTokens?>
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtManager _jwtManager;
    private readonly ITokenRepository _tokenRepository;
    private readonly IHasherPassword _hasher;

    public GenerateTokenCommandHandler(
        IUserRepository userRepository,
        IJwtManager jwtManager,
        ITokenRepository tokenRepository,
        IHasherPassword hasher)
    {
        _userRepository = userRepository;
        _jwtManager = jwtManager;
        _tokenRepository = tokenRepository;
        _hasher = hasher;
    }
    
    public async Task<AuthTokens?> Handle(GenerateTokenCommand request, CancellationToken cancellationToken)
    {
        var principal = _jwtManager.GetClaimsPrincipal(request.AccessToken);
        var userEmail = principal.FindFirst(ClaimTypes.Email);

        if (userEmail is null)
            return null;
        
        var user = await _userRepository.FindUserByEmailAsync(userEmail.Value);

        if (user is null || !user.IsSubmittedEmail)
            return null;

        var oldRefreshToken = await _tokenRepository.GetRefreshTokenByValue(new Token(request.RefreshToken));
        
        // cant find token or token was used in the past
        if (oldRefreshToken is null || oldRefreshToken.IsUsed)
            return null;
        
        // refresh token was recent, mark it as used => no one can use this token, because is invalid
        oldRefreshToken.SetUsed();
        await _tokenRepository.UpdateRefreshToken(oldRefreshToken);
        
        (string accessToken, RefreshToken refreshToken) = TokenUtils.CreateAuthPair(_jwtManager, user);

        var isAdded = await _tokenRepository.AddRefreshToken(refreshToken);
        if (!isAdded)
            return null;

        return new AuthTokens
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken.Token.Value
        };
    }
}