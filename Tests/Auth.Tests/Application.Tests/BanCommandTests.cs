using AuthService.Application.Commands.BanUser;
using AuthService.Domain.Errors;
using AuthService.Domain.Errors.UserErrors;
using AuthService.Domain.Models.Shared;
using AuthService.Domain.Models.UserAggregate;
using AuthService.Domain.Models.UserAggregate.Enumerations;
using AuthService.Domain.Models.UserAggregate.Repos;
using AuthService.Domain.Models.UserAggregate.ValueObjects;
using AuthService.Domain.Models.UserHistoryAggregate;
using AuthService.Domain.Models.UserHistoryAggregate.Repos;
using AutoFixture;
using DomainDriverDesignAbstractions;
using Moq;

namespace Application.Tests;

public class BanCommandTests
{
    private static readonly User ValidUser = User.Create(
        Guid.NewGuid().ToString(),
        "testname",
        "email@mail.ru",
        "nickname",
        18,
        "password123",
        "123456",
        RoleType.ActivatedUser.Name).Value;
    
    private static readonly History ValidHistory = History.Create(
        Guid.NewGuid().ToString(),
        Guid.NewGuid().ToString()).Value;
    
    [Fact]
    public async void HandleBanUser_WhenUserNotFound()
    {
        // Arrange 
        var mockHistoryRepository = new Mock<IUserHistoryRepository>();
        var mockUserRepository = new Mock<IUserRepository>();
        mockUserRepository
            .Setup(x => x.FindUserByEmailAsync(It.IsAny<Email>()))
            .ReturnsAsync((User?)null);
        
        var mockUnitOfWork = new Mock<IUnitOfWork>();
        
        var handler = new BanUserCommandHandler(
            mockHistoryRepository.Object,
            mockUserRepository.Object,
            mockUnitOfWork.Object);
        
        BanUserCommand banUserCommand = new Fixture()
            .Build<BanUserCommand>()
            .With(x => x.ViolatorEmail, "invalid_email")
            .Create();
        
        // Act
        var result = await handler.Handle(banUserCommand, default);

        // Assert 
        Assert.True(result.IsFailure);
        Assert.Equal(UserError.UserNotFound, result.Error);
        mockUserRepository.Verify(x =>
            x.FindUserByEmailAsync(It.IsAny<Email>()), Times.Once);
        
        mockHistoryRepository.Verify(x =>
            x.FindByUserId(It.IsAny<Id>()), Times.Never);
    }
    
    [Fact]
    public async void HandleBanUser_WhenHistoryNotFound()
    {
        // Arrange
        var mockHistoryRepository = new Mock<IUserHistoryRepository>();
        mockHistoryRepository.Setup(x => x.FindByUserId(It.IsAny<Id>()))
            .ReturnsAsync((History?)null);
        
        var mockUserRepository = new Mock<IUserRepository>();
        mockUserRepository
            .Setup(x => x.FindUserByEmailAsync(It.IsAny<Email>()))
            .ReturnsAsync(ValidUser);
        
        var mockUnitOfWork = new Mock<IUnitOfWork>();
        
        var handler = new BanUserCommandHandler(
            mockHistoryRepository.Object,
            mockUserRepository.Object,
            mockUnitOfWork.Object);
        
        BanUserCommand banUserCommand = new Fixture()
            .Build<BanUserCommand>()
            .Create();
        
        // Act
        var result = await handler.Handle(banUserCommand, default);

        // Assert 
        Assert.True(result.IsFailure);
        Assert.Equal(HistoryError.HistoryNotFound, result.Error);
        
        mockUserRepository.Verify(x =>
            x.FindUserByEmailAsync(It.IsAny<Email>()), Times.Once);
        
        mockHistoryRepository.Verify(x =>
            x.FindByUserId(It.IsAny<Id>()), Times.Once);
        
        mockUnitOfWork.Verify(x => x.SaveChangesAsync(default), Times.Never);
    }
    
    [Fact]
    public async void HandleDurationOfBan_ShouldBeGreaterThanUtcNow()
    {
        // Arrange
        var mockHistoryRepository = new Mock<IUserHistoryRepository>();
        mockHistoryRepository.Setup(x => x.FindByUserId(It.IsAny<Id>()))
            .ReturnsAsync(ValidHistory);
        
        var mockUserRepository = new Mock<IUserRepository>();
        mockUserRepository
            .Setup(x => x.FindUserByEmailAsync(It.IsAny<Email>()))
            .ReturnsAsync(ValidUser);
        
        var mockUnitOfWork = new Mock<IUnitOfWork>();
        
        var handler = new BanUserCommandHandler(
            mockHistoryRepository.Object,
            mockUserRepository.Object,
            mockUnitOfWork.Object);
        
        BanUserCommand banUserCommand = new Fixture()
            .Build<BanUserCommand>()
            .With(b => b.MinutesToBan, 10)
            .Create();
        
        // Act
        var result = await handler.Handle(banUserCommand, default);

        // Assert 
        Assert.True(result.IsSuccess);
        Assert.Equal(Error.None, result.Error);
        
        mockUserRepository.Verify(x =>
            x.FindUserByEmailAsync(It.IsAny<Email>()), Times.Once);
        
        mockHistoryRepository.Verify(x =>
            x.FindByUserId(It.IsAny<Id>()), Times.Once);
        
        mockHistoryRepository.Verify(x => 
            x.UpdateHistory(It.Is<History>(h => 
                h.BannedTime > DateTime.UtcNow)), Times.Once);
        mockUnitOfWork.Verify(x => x.SaveChangesAsync(default), Times.Once);
    }
}