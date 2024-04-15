using AuthService.Domain.Errors.UserErrors;
using AuthService.Domain.Models.UserAggregate.Enumerations;

namespace Domain.Tests;

public class UserTests
{
    private static string GuidId = "91846efd-cc96-4ebd-b1b9-8d95c7e1c24b";
    
    [Fact]
    public void HandleCreatingUser_WhenLess16Age_ShouldBeRejected()
    {
        // Arrange
        var userResult = User.Create(GuidId,
            "TestName",
            "test@mail.ru",
            new DateTime(2020, 4, 15),
            "123456",
            "TestSalt",
            RoleType.UnactivatedUser.Name);
        // Assert
        
        Assert.True(userResult.IsFailure);
        Assert.Equal(UserError.YoungUser, userResult.Error);
    }
    
    [Fact]
    public void HandleCreatingUser_WhenInvalidRole_ShouldBeUnactivated()
    {
        // Arrange
        var userResult = User.Create(GuidId,
            "TestName",
            "test@mail.ru",
            new DateTime(2003, 4, 15),
            "123456",
            "TestSalt",
            "error");
        // Assert
        
        Assert.True(userResult.IsSuccess);
        Assert.Equal(RoleType.UnactivatedUser, userResult.Value.Role);
    }
    
    [Fact]
    public void HandleCreatingUser_WhenInvalidEmail_ShouldBeRejected()
    {
        // Arrange
        var userResult = User.Create(GuidId,
            "TestName",
            "tesdasdasdamail.ru",
            new DateTime(2003, 4, 15),
            "123456",
            "TestSalt",
            RoleType.ActivatedUser.Name);
        // Assert
        
        Assert.True(userResult.IsFailure);
        Assert.Equal(UserError.InvalidEmail, userResult.Error);
    }
}