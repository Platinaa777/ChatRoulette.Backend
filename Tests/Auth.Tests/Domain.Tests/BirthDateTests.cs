using AuthService.Domain.Errors.UserErrors;
using AuthService.Domain.Models.UserAggregate.ValueObjects;
using DomainDriverDesignAbstractions;

namespace Domain.Tests;

public class BirthDateTests
{
    [Fact]
    public void Handle_WhenAgeLessThan16_ShouldBeFailure()
    {
        var dateUtc = BirthDateUtc.Create(
            new DateTime(2020, 4, 15));
        
        Assert.True(dateUtc.IsFailure);
        Assert.Equal(UserError.YoungUser, dateUtc.Error);
    }
    
    [Fact]
    public void Handle_WhenAgeLessGreater100_ShouldBeFailure()
    {
        var dateUtc = BirthDateUtc.Create(
            new DateTime(1900, 4, 15));
        
        Assert.True(dateUtc.IsFailure);
        Assert.Equal(UserError.OldUser, dateUtc.Error);
    }
    
    [Fact]
    public void Handle_WhenAgeLessBetween16And100_ShouldBeSuccess()
    {
        var dateUtc = BirthDateUtc.Create(
            new DateTime(2004, 4, 15));
        
        Assert.True(dateUtc.IsSuccess);
        Assert.Equal(Error.None, dateUtc.Error);
    }
}