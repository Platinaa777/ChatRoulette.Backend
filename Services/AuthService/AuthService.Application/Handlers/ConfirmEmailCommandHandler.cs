using AuthService.Application.Commands;
using AuthService.Domain.Models.UserAggregate.Repos;
using MediatR;

namespace AuthService.Application.Handlers;

public class ConfirmEmailCommandHandler : IRequestHandler<ConfirmEmailCommand, string>
{
    private readonly IUserRepository _userRepository;

    public ConfirmEmailCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    public async Task<string> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.FindUserByEmailAsync(request.Email);

        if (user == null)
            return null;

        var result = await _userRepository.UpdateUserAsync(user);

        if (!result)
            return null;
        
        return user.Id;
    }
}