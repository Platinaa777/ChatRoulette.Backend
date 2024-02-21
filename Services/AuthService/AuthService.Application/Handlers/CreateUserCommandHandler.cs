using AuthService.Application.BusMapper;
using AuthService.Application.Commands;
using AuthService.Domain.Models.UserAggregate.Repos;
using AuthService.Infrastructure.Security;
using MassTransit.Client.EventBus;
using MediatR;

namespace AuthService.Application.Handlers;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, bool>
{
    private readonly IUserRepository _userRepository;
    private readonly IEventBusClient _eventBusClient;

    public CreateUserCommandHandler(IUserRepository userRepository, IEventBusClient eventBusClient)
    {
        _userRepository = userRepository;
        _eventBusClient = eventBusClient;
    }
    
    public async Task<bool> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = request.ToDomain();
        var response = await _userRepository.AddUserAsync(user);

        if (response)
            await _eventBusClient.PublishAsync(user.ToBusMessage(), cancellationToken);

        return response;
    }
}