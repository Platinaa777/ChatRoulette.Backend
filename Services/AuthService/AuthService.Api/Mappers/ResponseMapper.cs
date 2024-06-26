using AuthService.Application.Models;
using AuthService.HttpModels.Responses;
using DomainDriverDesignAbstractions;

namespace AuthService.Api.Mappers;

public static class ResponseMapper
{
    public static AuthenticationResponse NotFoundRefreshToken = new AuthenticationResponse(isAuthenticate: false, email: "", new ErrorInfo("400", "Not found refresh token"));
    public static AuthenticationResponse ToAnswer(this Result<AuthTokens> result)
    {
        if (result.IsFailure)
        {
            return new AuthenticationResponse(
                isAuthenticate: false, 
                string.Empty,
                string.Empty);
        }

        return new AuthenticationResponse(
            isAuthenticate: true,
            result.Value.Email,
            result.Value.AccessToken!);
    }    
}