using AuthService.Application.Commands;
using AuthService.Domain.Models.UserAggregate.Repos;
using AuthService.Domain.Models.UserAggregate.ValueObjects;
using AuthService.HttpModels.Responses;
using AuthService.Infrastructure.JwtGenerator;
using MediatR;

namespace AuthService.Application.Handlers;

public class GenerateTokenCommandHandler : IRequestHandler<GenerateTokenCommand, AuthenticationResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly JwtTokenCreator _jwtCreator;

    public GenerateTokenCommandHandler(IUserRepository userRepository, JwtTokenCreator jwtCreator)
    {
        _userRepository = userRepository;
        _jwtCreator = jwtCreator;
    }
    
    public async Task<AuthenticationResponse> Handle(GenerateTokenCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.FindUserByEmailAsync(request.Email);

        if (user == null)
            return new AuthenticationResponse() { IsAuthenticate = false };

        if (!user.PasswordHash.Equals(new Password(request.Password)))
            return new AuthenticationResponse() { IsAuthenticate = false };

        var token = _jwtCreator.CreateToken(user);
        return new AuthenticationResponse() { IsAuthenticate = true, Role = user.Role.Name, Token = token };
    }
}