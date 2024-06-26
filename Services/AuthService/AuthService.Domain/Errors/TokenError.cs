using DomainDriverDesignAbstractions;

namespace AuthService.Domain.Errors;

public class TokenError : Error
{
    public static readonly Error EmptyToken = new("Token.Error", "Token cant be empty or null");
    public static readonly Error EmptyUserId = new("Token.Error", "User id cant be empty or null");
    public static readonly Error InvalidExpiredTime = new("Token.Error.Time", "Invalid expired time");
    public static readonly Error EmailClaimsNotFound = new("Token.Error.Jwt", "Email not found in claims");
    public static readonly Error InvalidRefreshToken = new("Token.Error", "Invalid refresh token");
    public static readonly Error AddError = new("Token.Error.Repo", "Error with adding token");
    public static readonly Error UpdateError = new("Token.Error.Repo", "Error with updating token");
    public static readonly Error InvalidRefreshId = new("Token.Error", "Invalid refresh token id");

    
    public TokenError(string code, string message) : base(code, message) { }
}