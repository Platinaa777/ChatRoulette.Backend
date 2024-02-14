using AuthService.Application.Commands;
using AuthService.Domain.Models.UserAggregate.Repos;
using MediatR;

namespace AuthService.Application.Handlers;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, bool>
{
    private readonly IUserRepository _userRepository;

    public CreateUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    public async Task<bool> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = request.ToDomain();
        var response = await _userRepository.AddUserAsync(user);

        return response;
    }
}