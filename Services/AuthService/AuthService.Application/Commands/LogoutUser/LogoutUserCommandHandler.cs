using AuthService.Application.Models;
using AuthService.Domain.Errors.TokenErrors;
using AuthService.Domain.Errors.UserErrors;
using AuthService.Domain.Models.TokenAggregate.Repos;
using AuthService.Domain.Models.TokenAggregate.ValueObjects.Token;
using AuthService.Domain.Models.UserAggregate.Repos;
using AuthService.Domain.Shared;
using MediatR;

namespace AuthService.Application.Commands.LogoutUser;

public class LogoutUserCommandHandler : IRequestHandler<LogoutUserCommand, Result>
{
    private readonly ITokenRepository _tokenRepository;
    private readonly IUserRepository _userRepository;

    public LogoutUserCommandHandler(
        ITokenRepository tokenRepository,
        IUserRepository userRepository)
    {
        _tokenRepository = tokenRepository;
        _userRepository = userRepository;
    }
    
    public async Task<Result> Handle(LogoutUserCommand request, CancellationToken cancellationToken)
    {
        var tokenResult = Token.Create(request.RefreshToken);
        if (tokenResult.IsFailure)
            return Result.Failure(tokenResult.Error);

        var storedToken = await _tokenRepository.GetRefreshTokenByValue(tokenResult.Value);
        
        if (storedToken is null || storedToken.WasUsed() || storedToken.IsExpired())
            return Result.Failure(TokenError.InvalidRefreshToken);
        
        var user = await _userRepository.FindUserByIdAsync(storedToken.UserId.Value);

        if (user is null || !user.IsSubmittedEmail)
            return Result.Failure<AuthTokens>(UserError.UnactivatedUser);
        
        // refresh token was recent, mark it as used => no one can use this token, because is invalid
        storedToken.SetUsed();
        
        return Result.Create(await _tokenRepository.UpdateRefreshToken(storedToken));
    }
}