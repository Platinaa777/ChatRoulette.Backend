using AuthService.Application.Commands.ConfirmEmail;
using AuthService.Domain.Errors.UserErrors;
using AuthService.Domain.Models.UserAggregate;
using AuthService.Domain.Models.UserAggregate.Enumerations;
using AuthService.Domain.Models.UserAggregate.Repos;
using AuthService.Domain.Models.UserAggregate.ValueObjects;
using AutoFixture;
using DomainDriverDesignAbstractions;
using Moq;

namespace Application.Tests;

public class ConfirmEmailCommandTests
{
    [Fact]
    public async void HandlerConfirmEmail_WhenEmailInvalid_ShouldBeInvalidEmailError()
    {
        // Arrange 
        var mockUserRepository = new Mock<IUserRepository>();
        var mockUnitOfWork = new Mock<IUnitOfWork>();
        
        var handler = new ConfirmEmailCommandHandler(
            mockUserRepository.Object,
            mockUnitOfWork.Object);
        
        ConfirmEmailCommand confirmEmailCommand = new Fixture()
            .Build<ConfirmEmailCommand>()
            .With(x => x.Email, "miro312312mail.ru")
            .Create();
        
        // Act
        var result = await handler.Handle(confirmEmailCommand, default);

        // Assert 
        Assert.True(result.IsFailure);
        Assert.Equal(UserError.InvalidEmail, result.Error);
        
        mockUserRepository.Verify(x => 
            x.FindUserByEmailAsync(It.IsAny<Email>()), Times.Never);
    }
    
    [Fact]
    public async void HandlerConfirmEmail_WhenUserWithEmailNotExist_ShouldBeUserNotFound()
    {
        // Arrange 
        var mockUserRepository = new Mock<IUserRepository>();
        var mockUnitOfWork = new Mock<IUnitOfWork>();
        
        var handler = new ConfirmEmailCommandHandler(
            mockUserRepository.Object,
            mockUnitOfWork.Object);
        
        ConfirmEmailCommand confirmEmailCommand = new Fixture()
            .Build<ConfirmEmailCommand>()
            .With(x => x.Email, "test@mail.ru")
            .Create();
        
        // Act
        var result = await handler.Handle(confirmEmailCommand, default);

        // Assert 
        Assert.True(result.IsFailure);
        
        mockUserRepository.Verify(x => 
            x.FindUserByEmailAsync(It.IsAny<Email>()), Times.Once);
        
        Assert.Equal(UserError.UserNotFound, result.Error);
    }
    
    [Fact]
    public async void HandlerConfirmEmail_WhenUserSubmitEmail_ShouldReturnsUserId()
    {
        // Arrange 
        var userTest = User.Create(Guid.NewGuid().ToString(),
            "testname",
            "test@mail.ru",
            "nickname",
            19,
            "123456789",
            "1231231",
            RoleType.UnactivatedUser.Name).Value;
        
        var mockUserRepository = new Mock<IUserRepository>();
        mockUserRepository.Setup(x => 
                x.FindUserByEmailAsync(userTest.Email))
                .ReturnsAsync(userTest);
        mockUserRepository.Setup(x => x.UpdateUserAsync(userTest))
            .ReturnsAsync(true);
        
        var mockUnitOfWork = new Mock<IUnitOfWork>();
        
        var handler = new ConfirmEmailCommandHandler(
            mockUserRepository.Object,
            mockUnitOfWork.Object);
        
        ConfirmEmailCommand confirmEmailCommand = new Fixture()
            .Build<ConfirmEmailCommand>()
            .With(x => x.Email, userTest.Email.Value)
            .Create();
        
        // Act
        var result = await handler.Handle(confirmEmailCommand, default);

        // Assert 
        Assert.True(result.IsSuccess);
        
        mockUserRepository.Verify(x => 
            x.FindUserByEmailAsync(It.IsAny<Email>()), Times.Once);
        mockUserRepository.Verify(x => 
                x.UpdateUserAsync(It.IsAny<User>()), Times.Once);
        mockUnitOfWork.Verify(x => x.SaveChangesAsync(default), Times.Once);
        
        Assert.True(userTest.IsSubmittedEmail);
        Assert.Equal(userTest.Id.Value, result.Value);
    }
}