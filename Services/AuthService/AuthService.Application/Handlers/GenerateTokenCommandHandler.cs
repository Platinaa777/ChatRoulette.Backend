using AuthService.Application.Commands;
using AuthService.Domain.Models.UserAggregate.Repos;
using AuthService.Domain.Models.UserAggregate.ValueObjects;
using AuthService.HttpModels.Responses;
using AuthService.Infrastructure.JwtGenerator;
using AuthService.Infrastructure.Security;
using MediatR;

namespace AuthService.Application.Handlers;

public class GenerateTokenCommandHandler : IRequestHandler<GenerateTokenCommand, AuthenticationResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly JwtTokenCreator _jwtCreator;
    private readonly IPasswordRepository _passwords;

    public GenerateTokenCommandHandler(IUserRepository userRepository, JwtTokenCreator jwtCreator, IPasswordRepository passwords)
    {
        _userRepository = userRepository;
        _jwtCreator = jwtCreator;
        _passwords = passwords;
    }
    
    public async Task<AuthenticationResponse> Handle(GenerateTokenCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.FindUserByEmailAsync(request.Email);

        if (user == null)
            return new AuthenticationResponse() { IsAuthenticate = false };
        
        var saltDb = await _passwords.FindSaltByUserId(user.Id);
        var hashedPassword = PasswordHasher.HashPasswordWithSalt(request.Password, saltDb);
        
        if (!PasswordHasher.Verify(user.PasswordHash.Value, hashedPassword))
            return new AuthenticationResponse() { IsAuthenticate = false };

        var token = _jwtCreator.CreateToken(user);
        return new AuthenticationResponse() { IsAuthenticate = true, Role = user.Role.Name, Token = token };
    }
}