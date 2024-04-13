using AuthService.Application.Cache;
using AuthService.Application.Commands.CreateUser;
using AuthService.Application.Security;
using AuthService.Domain.Errors.UserErrors;
using AuthService.Domain.Models.UserAggregate;
using AuthService.Domain.Models.UserAggregate.Repos;
using AutoFixture;
using DomainDriverDesignAbstractions;
using MassTransit.Client.EventBus;
using MassTransit.Contracts.UserEvents;
using Moq;

namespace Application.Tests;

public class CreateUserCommandTests
{
    [Fact]
    public async void HandlerCreateUser_WhenAlreadyExists_ShouldBeAlreadyUserExistErrorOccured()
    {
        // Arrange
        var mockUserRepository = new Mock<IUserRepository>();
        var mockEventBus = new Mock<IEventBusClient>();
        var mockCache = new Mock<ICacheStorage>();
        var mockHasher = new Mock<IHasherPassword>();
        var mockUnitOfWork = new Mock<IUnitOfWork>();

        var command = new Fixture()
            .Build<CreateUserCommand>()
            .With(x => x.Email, "test@mail.ru")
            .With(x => x.Age, 19)
            .Create();

        mockHasher.Setup(x => x.GenerateSalt())
            .Returns("test-salt");
        mockHasher.Setup(x => x.HashPasswordWithSalt(It.IsAny<string>(), It.IsAny<string>()))
            .Returns("123456789");

        mockUserRepository.Setup(x => x.AddUserAsync(It.IsAny<User>()))
            .ReturnsAsync(false);

        var handler = new CreateUserCommandHandler(
            mockUserRepository.Object,
            mockEventBus.Object,
            mockCache.Object,
            mockHasher.Object,
            mockUnitOfWork.Object);
        
        // Act

        var result = await handler.Handle(command, default); 
        
        // Asser
        Assert.True(result.IsFailure);
        Assert.Equal(UserError.UserAlreadyExist, result.Error);
        
        mockEventBus.Verify(x => x.PublishAsync(It.IsAny<UserRegistered>(), default), Times.Never);
    }
    
    
    [Fact]
    public async void HandlerCreateUser_WhenUserWithSomeEmailNotExist_ShouldSaveInDataStorage()
    {
        // Arrange
        var mockUserRepository = new Mock<IUserRepository>();
        var mockEventBus = new Mock<IEventBusClient>();
        var mockCache = new Mock<ICacheStorage>();
        var mockHasher = new Mock<IHasherPassword>();
        var mockUnitOfWork = new Mock<IUnitOfWork>();

        var command = new Fixture()
            .Build<CreateUserCommand>()
            .With(x => x.Email, "test@mail.ru")
            .With(x => x.Age, 19)
            .Create();

        mockHasher.Setup(x => x.GenerateSalt())
            .Returns("test-salt");
        mockHasher.Setup(x => x.HashPasswordWithSalt(It.IsAny<string>(), It.IsAny<string>()))
            .Returns("123456789");

        mockUserRepository.Setup(x => x.AddUserAsync(It.IsAny<User>()))
            .ReturnsAsync(true);

        mockEventBus.Setup(x => x.PublishAsync(It.IsAny<UserRegistered>(), default))
            .Returns(Task.CompletedTask);

        mockCache.Setup(x => x.SetAsync(It.IsAny<string>(), It.IsAny<string>(), default))
            .Returns(Task.CompletedTask);

        var handler = new CreateUserCommandHandler(
            mockUserRepository.Object,
            mockEventBus.Object,
            mockCache.Object,
            mockHasher.Object,
            mockUnitOfWork.Object);
        
        // Act
        var result = await handler.Handle(command, default); 
        
        // Asser
        Assert.True(result.IsSuccess);
        Assert.Equal(Error.None, result.Error);
        mockEventBus.Verify(x => x.PublishAsync(It.IsAny<UserRegistered>(), default), Times.Once);
    }
}