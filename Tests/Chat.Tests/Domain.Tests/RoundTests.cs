using Chat.Domain.Aggregates.Game;

namespace Domain.Tests;

public class RoundTests
{
    [Fact]
    public void HandleRound_WhenAnyoneChooseCorrectWordTranslation_ShouldReturnNullEmail()
    {
        Round round = new Round(Guid.NewGuid(), "hello", "1@mail.ru", "2@mail.ru");
        
        round.SetAnswer("1@mail.ru", "hello1");
        round.SetAnswer("2@mail.ru", "hello2");

        Assert.Null(round.GetWinnerEmail());
        Assert.False(round.IsHasCertainWinner());
    }
    
    [Fact]
    public void HandleRound_WhenFirstChooseCorrectWordTranslation_ShouldReturnFirstPlayerEmail()
    {
        Round round = new Round(Guid.NewGuid(), "hello", "1@mail.ru", "2@mail.ru");
        
        round.SetAnswer("1@mail.ru", "hello");
        round.SetAnswer("2@mail.ru", "hello2");

        Assert.Equal("1@mail.ru",round.GetWinnerEmail());
        Assert.True(round.IsHasCertainWinner());
    }
    
    [Fact]
    public void HandleRound_WhenSecondChooseCorrectWordTranslation_ShouldReturnSecondPlayerEmail()
    {
        Round round = new Round(Guid.NewGuid(), "hello", "1@mail.ru", "2@mail.ru");
        
        round.SetAnswer("1@mail.ru", "hello1");
        round.SetAnswer("2@mail.ru", "hello");

        Assert.Equal("2@mail.ru",round.GetWinnerEmail());
        Assert.True(round.IsHasCertainWinner());
    }
    
    [Fact]
    public void HandleRound_WhenZeroAnswers_ShouldReturnNullEmail()
    {
        Round round = new Round(Guid.NewGuid(), "hello", "1@mail.ru", "2@mail.ru");
        
        Assert.Null(round.GetWinnerEmail());
        Assert.False(round.IsHasCertainWinner());
    }
    
    [Fact]
    public void HandleRound_WhenBothChooseCorrectButFirstPlayerWasEarlier_ShouldReturnFirstPlayerEmail()
    {
        Round round = new Round(Guid.NewGuid(), "hello", "1@mail.ru", "2@mail.ru");
        
        round.SetAnswer("1@mail.ru", "hello");
        round.SetAnswer("2@mail.ru", "hello");
        
        Assert.NotNull(round.GetWinnerEmail());
        Assert.Equal("1@mail.ru", round.GetWinnerEmail());
        Assert.True(round.IsHasCertainWinner());
    }
    
    [Fact]
    public void HandleRound_WhenFirstPlayerChooseCorrectAndSecondNotChooseAnyAnswers_ShouldReturnFirstPlayerEmail()
    {
        Round round = new Round(Guid.NewGuid(), "hello", "1@mail.ru", "2@mail.ru");
        
        round.SetAnswer("1@mail.ru", "hello");
        
        Assert.NotNull(round.GetWinnerEmail());
        Assert.Equal("1@mail.ru", round.GetWinnerEmail());
        Assert.True(round.IsHasCertainWinner());
    }
    
    [Fact]
    public void HandleRound_WhenSecondPlayerChooseCorrectAndFirstNotChooseAnyAnswers_ShouldReturnSecondPlayerEmail()
    {
        Round round = new Round(Guid.NewGuid(), "hello", "1@mail.ru", "2@mail.ru");
        
        round.SetAnswer("2@mail.ru", "hello");
        
        Assert.NotNull(round.GetWinnerEmail());
        Assert.Equal("2@mail.ru", round.GetWinnerEmail());
        Assert.True(round.IsHasCertainWinner());
    }
}