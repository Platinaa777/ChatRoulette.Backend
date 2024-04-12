using DomainDriverDesignAbstractions;

namespace Chat.Domain.Errors;

public class ChatUserErrors : Error
{
    public static Error NotFound = new("ChatUser.Error", "User not found");
    
    public ChatUserErrors(string code, string message) : base(code, message)
    {
    }
}