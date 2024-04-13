using AuthService.Domain.Models.UserHistoryAggregate;
using FluentAssertions;
using Xunit.Abstractions;

namespace Domain.Tests;

public class HistoryTests
{
    private readonly ITestOutputHelper _testOutputHelper;
    private static readonly string GuidId = "91846efd-cc96-4ebd-b1b9-8d95c7e1c24b";

    public HistoryTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public void HandleCreatingHistory_WhenBanTimeLessThanUtcNow_ShouldBeNullBanTime()
    {
        var historyResult = History.Create(
            GuidId,
            GuidId,
            DateTime.UtcNow.Subtract(TimeSpan.FromHours(1)));
        
        Assert.Null(historyResult.Value.BannedTime);
    }
    
    [Fact]
    public void HandleCreatingHistory_WhenBanTimeGreaterThanUtcNow_ShouldBeNotNullBanTime()
    {
        var historyResult = History.Create(
            GuidId,
            GuidId,
            DateTime.UtcNow.AddHours(3));
        
        Assert.NotNull(historyResult.Value.BannedTime);
        historyResult.Value.BannedTime.Should().BeAfter(DateTime.UtcNow);
    }
    
    [Fact]
    public void HandleCreatingHistory_WhenBanUser_ShouldBeTimeGreaterThanUtcNow()
    {
        var historyResult = History.Create(
            GuidId,
            GuidId);
        
        Assert.True(historyResult.IsSuccess);
        Assert.Null(historyResult.Value.BannedTime);

        historyResult.Value.BanUser(20);
        
        Assert.NotNull(historyResult.Value.BannedTime);
        historyResult.Value.BannedTime.Should().BeAfter(DateTime.UtcNow);
    }
}