using AuthService.Application.Models;
using AuthService.Application.Security;
using AuthService.Application.Utils;
using AuthService.Domain.Errors.TokenErrors;
using AuthService.Domain.Errors.UserErrors;
using AuthService.Domain.Models.TokenAggregate.Repos;
using AuthService.Domain.Models.TokenAggregate.ValueObjects;
using AuthService.Domain.Models.UserAggregate.Repos;
using AuthService.Domain.Models.UserAggregate.ValueObjects;
using AuthService.Domain.Models.UserHistoryAggregate.Repos;
using DomainDriverDesignAbstractions;
using MediatR;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AuthService.Application.Commands.LoginUser;

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, Result<AuthTokens>>
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenRepository _tokenRepository;
    private readonly IHasherPassword _hasherPassword;
    private readonly IJwtManager _jwtManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserHistoryRepository _historyRepository;

    public LoginUserCommandHandler(
        IUserRepository userRepository,
        ITokenRepository tokenRepository,
        IHasherPassword hasherPassword,
        IJwtManager jwtManager,
        IUnitOfWork unitOfWork,
        IUserHistoryRepository historyRepository)
    {
        _userRepository = userRepository;
        _tokenRepository = tokenRepository;
        _hasherPassword = hasherPassword;
        _jwtManager = jwtManager;
        _unitOfWork = unitOfWork;
        _historyRepository = historyRepository;
    }
    
    public async Task<Result<AuthTokens>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var emailResult = Email.Create(request.Email);
        if (emailResult.IsFailure)
            return Result.Failure<AuthTokens>(emailResult.Error);
        
        var user = await _userRepository.FindUserByEmailAsync(emailResult.Value);

        if (user is null || !user.IsSubmittedEmail)
            return Result.Failure<AuthTokens>(UserError.UnactivatedUser);
        
        var history = await _historyRepository.FindByUserId(user.Id);
        
        if (history is not null && 
            history.BannedTime > DateTime.UtcNow)
        {
            return Result.Failure<AuthTokens>(UserError.BanUser);
        }

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

        var token = await _tokenRepository.FindValidRefreshTokenByUserId(user.Id);

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

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new AuthTokens(
            accessToken: authPairResult.Value.accessToken, 
            refreshToken: authPairResult.Value.refreshToken.Token.Value, 
            user.Email.Value);
    }
}