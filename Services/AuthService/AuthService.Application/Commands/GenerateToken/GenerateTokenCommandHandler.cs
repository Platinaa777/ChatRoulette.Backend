using System.Security.Claims;
using AuthService.Application.Models;
using AuthService.Application.Security;
using AuthService.Application.Utils;
using AuthService.Domain.Errors.TokenErrors;
using AuthService.Domain.Errors.UserErrors;
using AuthService.Domain.Models.TokenAggregate;
using AuthService.Domain.Models.TokenAggregate.Repos;
using AuthService.Domain.Models.TokenAggregate.ValueObjects;
using AuthService.Domain.Models.UserAggregate.Repos;
using AuthService.Domain.Shared;
using MediatR;

namespace AuthService.Application.Commands.GenerateToken;

public class GenerateTokenCommandHandler : IRequestHandler<GenerateTokenCommand, Result<AuthTokens>>
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtManager _jwtManager;
    private readonly ITokenRepository _tokenRepository;
    private readonly IUnitOfWork _unitOfWork;

    public GenerateTokenCommandHandler(
        IUserRepository userRepository,
        IJwtManager jwtManager,
        ITokenRepository tokenRepository,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _jwtManager = jwtManager;
        _tokenRepository = tokenRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Result<AuthTokens>> Handle(GenerateTokenCommand request, CancellationToken cancellationToken)
    {
        Result<Token> requestRefreshToken = Token.Create(request.RefreshToken);
        if (requestRefreshToken.IsFailure)
            return Result.Failure<AuthTokens>(requestRefreshToken.Error);

        var oldRefreshToken = await _tokenRepository.GetRefreshTokenByValue(requestRefreshToken.Value);
        
        // cant find token or token was used in the past
        if (oldRefreshToken is null || oldRefreshToken.IsUsed || oldRefreshToken.IsExpired())
            return Result.Failure<AuthTokens>(TokenError.InvalidRefreshToken);
        
        var user = await _userRepository.FindUserByIdAsync(oldRefreshToken.UserId.Value);

        if (user is null || !user.IsSubmittedEmail)
            return Result.Failure<AuthTokens>(UserError.UnactivatedUser);
        
        // refresh token was recent, mark it as used => no one can use this token, because is invalid
        oldRefreshToken.SetUsed();
        await _tokenRepository.UpdateRefreshToken(oldRefreshToken);
        
        var authPairResult = TokenUtils.CreateAuthPair(_jwtManager, user);

        if (authPairResult.IsFailure)
            return Result.Failure<AuthTokens>(authPairResult.Error);

        var isAdded = await _tokenRepository.AddRefreshToken(authPairResult.Value.refreshToken);
        if (!isAdded)
            return Result.Failure<AuthTokens>(TokenError.AddError);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new AuthTokens(
            authPairResult.Value.accessToken,
            authPairResult.Value.refreshToken.Token.Value,
            user.Email.Value);
    }
}