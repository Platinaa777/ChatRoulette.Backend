using AuthService.Application.BusMapper;
using AuthService.Application.Commands;
using AuthService.Domain.Models.UserAggregate.Repos;
using AuthService.Domain.Models.UserAggregate.ValueObjects;
using AuthService.Infrastructure.Cache;
using AuthService.Infrastructure.Cache.Models;
using AuthService.Infrastructure.Security;
using MassTransit.Client.EventBus;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace AuthService.Application.Handlers;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, bool>
{
    private readonly IUserRepository _userRepository;
    private readonly IEventBusClient _eventBusClient;
    private readonly IDistributedCache _cache;

    public CreateUserCommandHandler(IUserRepository userRepository, IEventBusClient eventBusClient, IDistributedCache cache)
    {
        _userRepository = userRepository;
        _eventBusClient = eventBusClient;
        _cache = cache;
    }
    
    public async Task<bool> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = request.ToDomain();
        var response = await _userRepository.AddUserAsync(user);
        
        user.AddUserExtraInformation(new Name(request.NickName), new Age(request.Age));
        
        if (response)
        {
            await _eventBusClient.PublishAsync(user.ToBusMessage(), cancellationToken);
            await _cache.SetStringAsync(user.Id, 
                JsonConvert.SerializeObject(user.ToCache(request.Preferences)),
                cancellationToken);
        }

        return response;
    }
}