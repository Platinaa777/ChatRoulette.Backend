using AuthService.Application.Commands;
using MassTransit;
using MassTransit.Contracts.UserEvents;
using MediatR;

namespace AuthService.Api.Consumers;

public class UserSubmitEmailConsumer : IConsumer<UserSubmittedEmail>
{
    private readonly IServiceScopeFactory _provider;
    private readonly ILogger<UserSubmittedEmail> _logger;

    public UserSubmitEmailConsumer(IServiceScopeFactory provider, ILogger<UserSubmittedEmail> logger)
    {
        _provider = provider;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<UserSubmittedEmail> context)
    {
        using (var scope = _provider.CreateScope())
        {
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            
            await mediator.Send(new ConfirmEmailCommand(context.Message.Email),
                context.CancellationToken);
            _logger.LogInformation($"Confirmation email: {context.Message.Email}");
        }
    }
}