using DomainDriverDesignAbstractions;

namespace ProfileService.Domain.Models.UserHistoryAggregate.Errors;

public class UserHistoryError : Error
{
    public static Error NegativeDoomSlayerPoints = new("UserHistory.Error", "DoomSlayer points cant be negative");
    
    public UserHistoryError(string code, string message) : base(code, message)
    {
    }
}