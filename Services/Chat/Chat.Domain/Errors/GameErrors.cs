using DomainDriverDesignAbstractions;

namespace Chat.Domain.Errors;

public class GameErrors : Error
{
    public static Error ZeroCountGames = new("Game.Error", "Zero count games at the moment");
    public static Error RoundNotFound = new("Game.Error", "Round with this id does not exist");
    public static Error InvalidRoundId = new("Game.Error", "Invalid value of id");

    public GameErrors(string code, string message) : base(code, message)
    {
    }
}