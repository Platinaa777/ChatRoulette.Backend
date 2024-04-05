using DomainDriverDesignAbstractions;

namespace AuthService.Domain.Errors;

public class HistoryError : Error
{
    public static readonly Error InvalidHistoryId = new("History.Error", "Invalid id");
    public static readonly Error HistoryNotFound = new("History.Error", "History not found");

    
    public HistoryError(string code, string message) : base(code, message)
    {
    }
}