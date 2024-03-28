using AuthService.Application.BusMapper;
using AuthService.Application.Cache;
using AuthService.Application.Cache.Models;
using AuthService.Application.Mappers;
using AuthService.Application.Models;
using AuthService.Application.Security;
using AuthService.Application.Utils;
using AuthService.Domain.Errors.UserErrors;
using AuthService.Domain.Models.TokenAggregate.Repos;
using AuthService.Domain.Models.UserAggregate;
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
    private readonly IEventBusClient _eventBusClient;
    private readonly ICacheStorage _cache;
    private readonly IHasherPassword _hasher;
    private readonly IUnitOfWork _unitOfWork;

    public CreateUserCommandHandler(
        IUserRepository userRepository,
        IEventBusClient eventBusClient,
        ICacheStorage cache,
        IHasherPassword hasher,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _eventBusClient = eventBusClient;
        _cache = cache;
        _hasher = hasher;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Result> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var userResult = request.ToDomain(_hasher);
        
        if (userResult.IsFailure)
            return Result.Failure(userResult.Error);

        var user = userResult.Value;
        
        var response = await _userRepository.AddUserAsync(userResult.Value);
        
        user.AddUserExtraInformation(user.NickName, user.Age);

        if (!response)
            return Result.Failure(UserError.UserAlreadyExist);
        
        // push to event bus
        await _eventBusClient.PublishAsync(user.ToBusMessage(), cancellationToken);
        // cache 
        await _cache.SetAsync(user.Id, 
            value: JsonConvert.SerializeObject(user.ToCache()), 
            cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Result.Success();
    }
}