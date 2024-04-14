using DomainDriverDesignAbstractions;

namespace Chat.Domain.Aggregates.Game;

public class Round : AggregateRoot<Guid>
{
    public Round(
        Guid id,
        string correctWord,
        string firstPlayerEmail,
        string secondPlayerEmail,
        string firstPlayerAnswer = EmptyAnswer,
        string secondPlayerAnswer = EmptyAnswer,
        DateTime? firstPlayerAnswerTime = null,
        DateTime? secondPlayerAnswerTime = null)
    {
        Id = id;
        CorrectWord = correctWord;
        FirstPlayerEmail = firstPlayerEmail;
        FirstPlayerAnswerTime = firstPlayerAnswerTime;
        FirstPlayerAnswer = firstPlayerAnswer;
        SecondPlayerEmail = secondPlayerEmail;
        SecondPlayerAnswerTime = secondPlayerAnswerTime;
        SecondPlayerAnswer = secondPlayerAnswer;
    }

    public void SetAnswer(string email, string answer)
    {
        if (FirstPlayerEmail == email)
        {
            FirstPlayerAnswer = answer;
            FirstPlayerAnswerTime = DateTime.UtcNow;
        } 
        if (SecondPlayerEmail == email)
        {
            SecondPlayerAnswer = answer;
            SecondPlayerAnswerTime = DateTime.UtcNow;    
        }
    }

    public string? GetWinnerEmail()
    {
        if (FirstPlayerAnswer != CorrectWord && SecondPlayerAnswer != CorrectWord)
            return null;

        if (FirstPlayerAnswer == CorrectWord && SecondPlayerAnswer == CorrectWord &&
            FirstPlayerAnswerTime == SecondPlayerAnswerTime)
            return "draw";

        if (FirstPlayerAnswer == CorrectWord && SecondPlayerAnswer != CorrectWord)
            return FirstPlayerEmail;
        
        if (FirstPlayerAnswer != CorrectWord && SecondPlayerAnswer == CorrectWord)
            return SecondPlayerEmail;

        if (FirstPlayerAnswer == CorrectWord && SecondPlayerAnswer == CorrectWord &&
            FirstPlayerAnswerTime > SecondPlayerAnswerTime)
            return SecondPlayerEmail;

        return FirstPlayerEmail;
    }

    public bool IsHasCertainWinner()
    {
        var winner = GetWinnerEmail();

        return winner is not null && winner != "draw";
    }
        

    public string CorrectWord { get; }
    public string FirstPlayerEmail { get; private set; }
    public DateTime? FirstPlayerAnswerTime { get; private set; }
    public string? FirstPlayerAnswer { get; private set; }
    public string SecondPlayerEmail { get; private set; }
    public DateTime? SecondPlayerAnswerTime { get; private set; }
    public string? SecondPlayerAnswer { get; private set; }

    private const string EmptyAnswer = "";
    
    private Round() {}
}