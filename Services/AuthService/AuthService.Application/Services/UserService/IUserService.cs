using AuthService.HttpModels.Requests;
using AuthService.HttpModels.Responses;

namespace AuthService.Application.Services;

public interface IUserService
{
    Task<bool> RegisterAsync(RegisterRequest request);
    Task<AuthenticationResponse> GetTokenAsync(TokenRequest request);
}