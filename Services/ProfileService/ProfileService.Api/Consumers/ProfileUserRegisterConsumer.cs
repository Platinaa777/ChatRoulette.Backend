using MassTransit;
using MassTransit.Contracts.UserEvents;
using MediatR;
using ProfileService.Application.Commands;
using ProfileService.Application.Commands.AddUserProfile;

namespace ProfileService.Api.Consumers;

public class ProfileUserRegisterConsumer : IConsumer<UserFullyRegistered>
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<ProfileUserRegisterConsumer> _logger;

    public ProfileUserRegisterConsumer(
        IServiceScopeFactory scopeFactory,
        ILogger<ProfileUserRegisterConsumer> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }
    
    public async Task Consume(ConsumeContext<UserFullyRegistered> context)
    {
        using var scope = _scopeFactory.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        var result = await mediator.Send(context.Message.ToCommandFromMessage(),
            context.CancellationToken);

        if (result.IsSuccess)
            _logger.LogInformation("User was added: {@Email}", context.Message.Email);
        else
            _logger.LogError("User was NOT added: {@Email}", context.Message.Email);
    }
}