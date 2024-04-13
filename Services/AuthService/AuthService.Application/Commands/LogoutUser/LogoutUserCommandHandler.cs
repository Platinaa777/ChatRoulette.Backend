using AuthService.Application.Models;
using AuthService.Domain.Errors;
using AuthService.Domain.Errors.UserErrors;
using AuthService.Domain.Models.TokenAggregate.Repos;
using AuthService.Domain.Models.TokenAggregate.ValueObjects;
using AuthService.Domain.Models.UserAggregate.Repos;
using DomainDriverDesignAbstractions;
using MediatR;

namespace AuthService.Application.Commands.LogoutUser;

public class LogoutUserCommandHandler : IRequestHandler<LogoutUserCommand, Result>
{
    private readonly ITokenRepository _tokenRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public LogoutUserCommandHandler(
        ITokenRepository tokenRepository,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork)
    {
        _tokenRepository = tokenRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Result> Handle(LogoutUserCommand request, CancellationToken cancellationToken)
    {
        var tokenResult = Token.Create(request.RefreshToken);
        if (tokenResult.IsFailure)
            return Result.Failure(tokenResult.Error);

        var storedToken = await _tokenRepository.GetRefreshTokenByValue(tokenResult.Value);
        
        if (storedToken is null || storedToken.WasUsed() || storedToken.IsExpired())
            return Result.Failure(TokenError.InvalidRefreshToken);
        
        var user = await _userRepository.FindUserByIdAsync(storedToken.UserId);

        if (user is null || !user.IsSubmittedEmail)
            return Result.Failure<AuthTokens>(UserError.UnactivatedUser);
        
        // refresh token was recent, mark it as used => no one can use this token, because is invalid
        storedToken.SetUsed();
        
        if (!(await _tokenRepository.UpdateRefreshToken(storedToken)))
            return Result.Failure<AuthTokens>(TokenError.UpdateError);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Result.Create(true);
    }
}