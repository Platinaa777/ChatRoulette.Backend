using AuthService.Application.Models;
using AuthService.Application.Security;
using AuthService.Application.Utils;
using AuthService.Domain.Models.TokenAggregate;
using AuthService.Domain.Models.TokenAggregate.Repos;
using AuthService.Domain.Models.TokenAggregate.ValueObjects.Token;
using AuthService.Domain.Models.UserAggregate.Repos;
using MediatR;

namespace AuthService.Application.Commands.LoginUser;

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, AuthTokens>
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenRepository _tokenRepository;
    private readonly IHasherPassword _hasherPassword;
    private readonly IJwtManager _jwtManager;

    public LoginUserCommandHandler(
        IUserRepository userRepository,
        ITokenRepository tokenRepository,
        IHasherPassword hasherPassword,
        IJwtManager jwtManager)
    {
        _userRepository = userRepository;
        _tokenRepository = tokenRepository;
        _hasherPassword = hasherPassword;
        _jwtManager = jwtManager;
    }
    
    public async Task<AuthTokens> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.FindUserByEmailAsync(request.Email);

        if (user is null || !user.IsSubmittedEmail)
            return new AuthTokens(null, null);

        var userSalt = user.Salt.Value;
        var password = _hasherPassword.HashPasswordWithSalt(request.Password, userSalt);

        var isValidPassword = _hasherPassword.Verify(user.PasswordHash.Value, password);
        if (!isValidPassword)
            return new AuthTokens(null, null);
        
        (string accessToken, RefreshToken refreshToken) = TokenUtils.CreateAuthPair(_jwtManager, user);

        var token = await _tokenRepository.FindValidRefreshTokenByUserId(UserId.CreateId(user.Id));

        // should mark refresh as not valid anymore
        if (token is not null)
        {
            token.SetUsed();
            await _tokenRepository.UpdateRefreshToken(token);
        }

        // add new refresh token
        var isAdded = await _tokenRepository.AddRefreshToken(refreshToken);
        if (!isAdded)
            return new AuthTokens(null, null);

        return new AuthTokens(accessToken: accessToken, refreshToken: refreshToken.Token.Value);
    }
}