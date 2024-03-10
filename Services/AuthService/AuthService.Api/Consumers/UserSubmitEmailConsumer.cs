using AuthService.Application.Cache;
using AuthService.Application.Cache.Models;
using AuthService.Application.Commands;
using AuthService.Application.Commands.ConfirmEmail;
using MassTransit;
using MassTransit.Contracts.UserEvents;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace AuthService.Api.Consumers;

public class UserSubmitEmailConsumer : IConsumer<UserSubmittedEmail>
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<UserSubmittedEmail> _logger;
    private readonly ICacheStorage _cache;

    public UserSubmitEmailConsumer(
        IServiceScopeFactory scopeFactory,
        ILogger<UserSubmittedEmail> logger,
        ICacheStorage cache)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
        _cache = cache;
    }

    public async Task Consume(ConsumeContext<UserSubmittedEmail> context)
    {
        using (var scope = _scopeFactory.CreateScope())
        {
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            
            var responseId = await mediator.Send(new ConfirmEmailCommand(context.Message.Email),
                context.CancellationToken);
            _logger.LogInformation($"Confirmation email: {context.Message.Email}");

            var userDetails = await _cache.GetAsync(responseId, context.CancellationToken);

            if (userDetails == null)
            {
                _logger.LogError($"NULL USER DETAILS {context.Message.Email}");
                return;
            }

            var userObject = JsonConvert.DeserializeObject<UserInformation>(userDetails)!;
            await context.Publish(userObject.ToBusMessage());
        }
    }
}