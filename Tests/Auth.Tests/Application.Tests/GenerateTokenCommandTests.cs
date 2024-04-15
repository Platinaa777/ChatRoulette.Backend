using AuthService.Application.Commands.GenerateToken;
using AuthService.Application.Security;
using AuthService.Domain.Errors;
using AuthService.Domain.Errors.UserErrors;
using AuthService.Domain.Models.Shared;
using AuthService.Domain.Models.TokenAggregate;
using AuthService.Domain.Models.TokenAggregate.Repos;
using AuthService.Domain.Models.TokenAggregate.ValueObjects;
using AuthService.Domain.Models.UserAggregate;
using AuthService.Domain.Models.UserAggregate.Enumerations;
using AuthService.Domain.Models.UserAggregate.Repos;
using AuthService.Domain.Models.UserHistoryAggregate;
using AuthService.Domain.Models.UserHistoryAggregate.Repos;
using DomainDriverDesignAbstractions;
using MassTransit;
using Moq;

namespace Application.Tests;

public class GenerateTokenCommandTests
{
    [Fact]
    public async void HandlerGenerateToken_WhenTokenValueNotFound_ShouldReturnInvalidRefreshTokenError()
    {
        var mockUserRepository = new Mock<IUserRepository>();
        var mockJwtManager = new Mock<IJwtManager>();
        var mockTokenRepository = new Mock<ITokenRepository>();
        var mockIUnitOfWork = new Mock<IUnitOfWork>();
        var mockHistoryRepository = new Mock<IUserHistoryRepository>();

        var token = "test-token";

        mockTokenRepository.Setup(x => x.GetRefreshTokenByValue(It.IsAny<Token>()))
            .ReturnsAsync((RefreshToken?)null);

        var command = new GenerateTokenCommand() { RefreshToken = token };

        var handler = new GenerateTokenCommandHandler(
            mockUserRepository.Object,
            mockJwtManager.Object,
            mockTokenRepository.Object,
            mockIUnitOfWork.Object,
            mockHistoryRepository.Object);

        var result = await handler.Handle(command, default);
        
        Assert.True(result.IsFailure);
        Assert.Equal(TokenError.InvalidRefreshToken, result.Error);
    }

    [Fact]
    public async void HandlerGenerateToken_WhenTokenIsUsed_ShouldReturnInvalidRefreshTokenError()
    {
        var mockUserRepository = new Mock<IUserRepository>();
        var mockJwtManager = new Mock<IJwtManager>();
        var mockTokenRepository = new Mock<ITokenRepository>();
        var mockIUnitOfWork = new Mock<IUnitOfWork>();
        var mockHistoryRepository = new Mock<IUserHistoryRepository>();

        var token = "test-token";

        mockTokenRepository.Setup(x => x.GetRefreshTokenByValue(It.IsAny<Token>()))
            .ReturnsAsync(RefreshToken.Create(
                Guid.NewGuid().ToString(),
                token,
                DateTime.UtcNow.AddHours(1),
                true,
                Guid.NewGuid().ToString()).Value);

        var command = new GenerateTokenCommand() { RefreshToken = token };

        var handler = new GenerateTokenCommandHandler(
            mockUserRepository.Object,
            mockJwtManager.Object,
            mockTokenRepository.Object,
            mockIUnitOfWork.Object,
            mockHistoryRepository.Object);

        var result = await handler.Handle(command, default);
        
        Assert.True(result.IsFailure);
        Assert.Equal(TokenError.InvalidRefreshToken, result.Error);
    }
    
    [Fact]
    public async void HandlerGenerateToken_WhenUserNotSubmitEmail_ShouldReturnUnactivatedUserError()
    {
        var mockUserRepository = new Mock<IUserRepository>();
        var mockJwtManager = new Mock<IJwtManager>();
        var mockTokenRepository = new Mock<ITokenRepository>();
        var mockIUnitOfWork = new Mock<IUnitOfWork>();
        var mockHistoryRepository = new Mock<IUserHistoryRepository>();
        
        var user = User.Create(
            Guid.NewGuid().ToString(),
            "testname",
            "email@mail.ru",
            new DateTime(2003, 4, 15),
            "password123",
            "123456",
            RoleType.ActivatedUser.Name).Value;

        mockUserRepository.Setup(x => x.FindUserByIdAsync(It.IsAny<Id>()))
            .ReturnsAsync(user);

        var token = "test-token";

        mockTokenRepository.Setup(x => x.GetRefreshTokenByValue(It.IsAny<Token>()))
            .ReturnsAsync(RefreshToken.Create(
                Guid.NewGuid().ToString(),
                token,
                DateTime.UtcNow.AddHours(1),
                false,
                user.Id.Value).Value);

        var command = new GenerateTokenCommand() { RefreshToken = token };

        var handler = new GenerateTokenCommandHandler(
            mockUserRepository.Object,
            mockJwtManager.Object,
            mockTokenRepository.Object,
            mockIUnitOfWork.Object,
            mockHistoryRepository.Object);

        var result = await handler.Handle(command, default);
        
        Assert.True(result.IsFailure);
        Assert.Equal(UserError.UnactivatedUser, result.Error);
    }
    
    [Fact]
    public async void HandlerGenerateToken_WhenUserWasBanned_ShouldReturnBanErrorAndSaveChangesWithOldToken()
    {
        var mockUserRepository = new Mock<IUserRepository>();
        var mockJwtManager = new Mock<IJwtManager>();
        var mockTokenRepository = new Mock<ITokenRepository>();
        var mockIUnitOfWork = new Mock<IUnitOfWork>();
        var mockHistoryRepository = new Mock<IUserHistoryRepository>();
        
        var user = User.Create(
            Guid.NewGuid().ToString(),
            "testname",
            "email@mail.ru",
            new DateTime(2003, 4, 15),
            "password123",
            "123456",
            RoleType.ActivatedUser.Name).Value;

        mockUserRepository.Setup(x => x.FindUserByIdAsync(It.IsAny<Id>()))
            .ReturnsAsync(user);
        
        user.SubmitEmail();

        var token = "test-token";

        mockTokenRepository.Setup(x => x.GetRefreshTokenByValue(It.IsAny<Token>()))
            .ReturnsAsync(RefreshToken.Create(
                Guid.NewGuid().ToString(),
                token,
                DateTime.UtcNow.AddHours(1),
                false,
                user.Id.Value).Value);

        mockHistoryRepository.Setup(x => x.FindByUserId(It.IsAny<Id>()))
            .ReturnsAsync(History.Create(
                Guid.NewGuid().ToString(),
                user.Id.Value,
                DateTime.UtcNow.AddDays(1)).Value);

        var command = new GenerateTokenCommand() { RefreshToken = token };

        var handler = new GenerateTokenCommandHandler(
            mockUserRepository.Object,
            mockJwtManager.Object,
            mockTokenRepository.Object,
            mockIUnitOfWork.Object,
            mockHistoryRepository.Object);

        var result = await handler.Handle(command, default);
        
        Assert.True(result.IsFailure);
        Assert.Equal(UserError.BanUser, result.Error);
        mockIUnitOfWork.Verify(x => x.SaveChangesAsync(default), Times.Once);
    }
}