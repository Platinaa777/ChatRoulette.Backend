using AuthService.Application.Commands;
using AuthService.Application.Security;
using AuthService.Domain.Models.UserAggregate.Repos;
using AuthService.Domain.Models.UserAggregate.ValueObjects;
using AuthService.HttpModels.Responses;
using MediatR;

namespace AuthService.Application.Handlers;

public class GenerateTokenCommandHandler : IRequestHandler<GenerateTokenCommand, AuthenticationResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtManager _jwtCreator;
    private readonly IPasswordRepository _passwords;
    private readonly IHasherPassword _hasher;

    public GenerateTokenCommandHandler(IUserRepository userRepository, IJwtManager jwtManager, IPasswordRepository passwords, IHasherPassword hasher)
    {
        _userRepository = userRepository;
        _jwtCreator = jwtManager;
        _passwords = passwords;
        _hasher = hasher;
    }
    
    public async Task<AuthenticationResponse> Handle(GenerateTokenCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.FindUserByEmailAsync(request.Email);

        if (user == null)
            return new AuthenticationResponse() { IsAuthenticate = false };
        
        var saltDb = await _passwords.FindSaltByUserId(user.Id);
        var hashedPassword = _hasher.HashPasswordWithSalt(request.Password, saltDb);
        
        if (!_hasher.Verify(user.PasswordHash.Value, hashedPassword))
            return new AuthenticationResponse() { IsAuthenticate = false };

        var token = _jwtCreator.GenerateAccessToken(user);
        return new AuthenticationResponse() { IsAuthenticate = true, Role = user.Role.Name, Token = token };
    }
}