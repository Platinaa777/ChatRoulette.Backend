using AuthService.Application.Models;
using AuthService.Application.Security;
using AuthService.Application.Utils;
using AuthService.Domain.Errors.TokenErrors;
using AuthService.Domain.Errors.UserErrors;
using AuthService.Domain.Models.TokenAggregate;
using AuthService.Domain.Models.TokenAggregate.Repos;
using AuthService.Domain.Models.TokenAggregate.ValueObjects.Token;
using AuthService.Domain.Models.UserAggregate.Repos;
using AuthService.Domain.Shared;
using MediatR;

namespace AuthService.Application.Commands.LoginUser;

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, Result<AuthTokens>>
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
    
    public async Task<Result<AuthTokens>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.FindUserByEmailAsync(request.Email);

        if (user is null || !user.IsSubmittedEmail)
            return Result.Failure<AuthTokens>(UserError.UnactivatedUser);

        var userSalt = user.Salt.Value;
        var password = _hasherPassword.HashPasswordWithSalt(request.Password, userSalt);

        var isValidPassword = _hasherPassword.Verify(
            passwordDb: user.PasswordHash.Value, 
            requestPassword: password);
        
        if (!isValidPassword)
            return Result.Failure<AuthTokens>(UserError.WrongPassword);
        
        var authPairResult = TokenUtils.CreateAuthPair(_jwtManager, user);

        if (authPairResult.IsFailure)
            return Result.Failure<AuthTokens>(authPairResult.Error);

        var userIdResult = UserId.CreateId(user.Id);
        if (userIdResult.IsFailure)
            return Result.Failure<AuthTokens>(userIdResult.Error);

        var token = await _tokenRepository.FindValidRefreshTokenByUserId(userIdResult.Value);

        // should mark refresh as not valid anymore
        if (token is not null)
        {
            token.SetUsed();
            await _tokenRepository.UpdateRefreshToken(token);
        }

        // add new refresh token
        var isAdded = await _tokenRepository.AddRefreshToken(authPairResult.Value.refreshToken);
        if (!isAdded)
            return Result.Failure<AuthTokens>(TokenError.AddError);

        return new AuthTokens(
            accessToken: authPairResult.Value.accessToken, 
            refreshToken: authPairResult.Value.refreshToken.Token.Value, 
            user.Email.Value);
    }
}