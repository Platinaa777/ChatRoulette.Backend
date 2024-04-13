using AuthService.Domain.Errors;
using AuthService.Domain.Models.TokenAggregate;
using DomainDriverDesignAbstractions;

namespace Domain.Tests;

public class TokenTests
{
    private static string GuidId = "91846efd-cc96-4ebd-b1b9-8d95c7e1c24b";
    
    [Fact]
    public void HandleCreatingToken_WhenTokenEmpty_ShouldBeRejected()
    {
        var tokenResult = RefreshToken.Create(
            GuidId,
            "",
            DateTime.UtcNow.AddHours(1),
            false,
            GuidId);
        
        Assert.True(tokenResult.IsFailure);
        Assert.Equal(TokenError.EmptyToken, tokenResult.Error);
    }
    
    [Fact]
    public void HandleCreatingToken_WhenExpiredLessThanNow_ShouldBeRejected()
    {
        var tokenResult = RefreshToken.Create(
            GuidId,
            "token",
            DateTime.UtcNow.Subtract(TimeSpan.FromHours(1)),
            false,
            GuidId);
        
        Assert.True(tokenResult.IsFailure);
        Assert.Equal(TokenError.InvalidExpiredTime, tokenResult.Error);
    }
    
    [Fact]
    public void HandleCreatingToken_WhenTokenUsed_ShouldBeWasUsed()
    {
        var tokenResult = RefreshToken.Create(
            GuidId,
            "token",
            DateTime.UtcNow.AddHours(1),
            false,
            GuidId);
        
        Assert.True(tokenResult.IsSuccess);
        Assert.Equal(Error.None, tokenResult.Error);

        tokenResult.Value.SetUsed();
        
        Assert.True(tokenResult.Value.WasUsed());
    }
}