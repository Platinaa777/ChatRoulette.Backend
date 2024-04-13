using AuthService.Application.Commands.LoginUser;
using AuthService.Application.Security;
using AuthService.Domain.Errors.UserErrors;
using AuthService.Domain.Models.Shared;
using AuthService.Domain.Models.TokenAggregate.Repos;
using AuthService.Domain.Models.UserAggregate;
using AuthService.Domain.Models.UserAggregate.Enumerations;
using AuthService.Domain.Models.UserAggregate.Repos;
using AuthService.Domain.Models.UserAggregate.ValueObjects;
using AuthService.Domain.Models.UserHistoryAggregate;
using AuthService.Domain.Models.UserHistoryAggregate.Repos;
using DomainDriverDesignAbstractions;
using Moq;

namespace Application.Tests;

public class LoginCommandTests
{
    [Fact]
    public async void HandlerLogin_WhenUserWasBanned_ShouldReturnBanErrorAndSaveChangesWithOldToken()
    {
        var mockUserRepository = new Mock<IUserRepository>();
        var mockTokenRepository = new Mock<ITokenRepository>();
        var mockHasher = new Mock<IHasherPassword>();
        var mockJwtManager = new Mock<IJwtManager>();
        var mockUnitOfWork = new Mock<IUnitOfWork>();
        var mockHistoryRepository = new Mock<IUserHistoryRepository>();
        
        var user = User.Create(
            Guid.NewGuid().ToString(),
            "testname",
            "email@mail.ru",
            "nickname",
            18,
            "password123",
            "123456",
            RoleType.ActivatedUser.Name).Value;

        mockUserRepository.Setup(x => x.FindUserByEmailAsync(It.IsAny<Email>()))
            .ReturnsAsync(user);
        
        user.SubmitEmail();

        mockHistoryRepository.Setup(x => x.FindByUserId(It.IsAny<Id>()))
            .ReturnsAsync(History.Create(
                Guid.NewGuid().ToString(),
                Guid.NewGuid().ToString(),
                DateTime.UtcNow.AddDays(1)).Value);
        var command = new LoginUserCommand() { Email = "test@mail.ru", Password = "123456780" };

        var handler = new LoginUserCommandHandler(
            mockUserRepository.Object, mockTokenRepository.Object, mockHasher.Object, mockJwtManager.Object, mockUnitOfWork.Object, mockHistoryRepository.Object);

        var result = await handler.Handle(command, default);
        
        Assert.True(result.IsFailure);
        Assert.Equal(UserError.BanUser, result.Error);
        
        mockUnitOfWork.Verify(x => x.SaveChangesAsync(default), Times.Never);
    }
    
    [Fact]
    public async void HandlerLogin_WhenInvalidPassword()
    {
        var mockUserRepository = new Mock<IUserRepository>();
        var mockTokenRepository = new Mock<ITokenRepository>();
        var mockHasher = new Mock<IHasherPassword>();
        var mockJwtManager = new Mock<IJwtManager>();
        var mockUnitOfWork = new Mock<IUnitOfWork>();
        var mockHistoryRepository = new Mock<IUserHistoryRepository>();
        
        var user = User.Create(
            Guid.NewGuid().ToString(),
            "testname",
            "email@mail.ru",
            "nickname",
            18,
            "password123",
            "123456",
            RoleType.ActivatedUser.Name).Value;

        var pass = "123456789";

        mockHasher.Setup(x => x.HashPasswordWithSalt(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(pass);
        
        mockHasher.Setup(x => x.Verify(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(false);
        
        mockUserRepository.Setup(x => x.FindUserByEmailAsync(It.IsAny<Email>()))
            .ReturnsAsync(user);
        
        user.SubmitEmail();

        mockHistoryRepository.Setup(x => x.FindByUserId(It.IsAny<Id>()))
            .ReturnsAsync(History.Create(
                Guid.NewGuid().ToString(),
                Guid.NewGuid().ToString()).Value);
        var command = new LoginUserCommand() { Email = "test@mail.ru", Password = "123456780" };

        var handler = new LoginUserCommandHandler(
            mockUserRepository.Object, mockTokenRepository.Object, mockHasher.Object, mockJwtManager.Object, mockUnitOfWork.Object, mockHistoryRepository.Object);

        var result = await handler.Handle(command, default);
        
        Assert.True(result.IsFailure);
        Assert.Equal(UserError.WrongPassword, result.Error);
        
        mockUnitOfWork.Verify(x => x.SaveChangesAsync(default), Times.Never);
    }
}