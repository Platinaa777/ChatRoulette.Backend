using AuthService.Application.BusMapper;
using AuthService.Application.Cache;
using AuthService.Application.Cache.Models;
using AuthService.Application.Mappers;
using AuthService.Application.Models;
using AuthService.Application.Security;
using AuthService.Application.Utils;
using AuthService.Domain.Errors.UserErrors;
using AuthService.Domain.Models.TokenAggregate.Repos;
using AuthService.Domain.Models.UserAggregate.Repos;
using AuthService.Domain.Models.UserAggregate.ValueObjects;
using AuthService.Domain.Shared;
using MassTransit.Client.EventBus;
using MediatR;
using Newtonsoft.Json;

namespace AuthService.Application.Commands.CreateUser;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Result>
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenRepository _tokenRepository;
    private readonly IEventBusClient _eventBusClient;
    private readonly ICacheStorage _cache;
    private readonly IHasherPassword _hasher;
    private readonly IJwtManager _jwtManager;

    public CreateUserCommandHandler(
        IUserRepository userRepository,
        IEventBusClient eventBusClient,
        ICacheStorage cache,
        IHasherPassword hasher,
        IJwtManager jwtManager,
        ITokenRepository tokenRepository)
    {
        _userRepository = userRepository;
        _eventBusClient = eventBusClient;
        _cache = cache;
        _hasher = hasher;
        _jwtManager = jwtManager;
        _tokenRepository = tokenRepository;
    }
    
    public async Task<Result> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = request.ToDomain(_hasher);
        var response = await _userRepository.AddUserAsync(user);
        
        user.AddUserExtraInformation(
            Name.Create(request.NickName).Value,
            Age.Create(request.Age).Value);

        if (!response)
            return Result.Failure(UserError.UserAlreadyExist);
        
        // push to event bus
        await _eventBusClient.PublishAsync(user.ToBusMessage(), cancellationToken);
        // cache 
        await _cache.SetAsync(user.Id, 
            value: JsonConvert.SerializeObject(user.ToCache(request.Preferences)), 
            cancellationToken);

        return Result.Success();
    }
}