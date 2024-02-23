using AuthService.Application.Commands;
using MassTransit;
using MassTransit.Contracts.UserEvents;
using MassTransit.Mediator;

namespace AuthService.Api.Consumers;

public class UserSubmitEmailConsumer : IConsumer<UserSubmittedEmail>
{
    private readonly IMediator _mediator;
    private readonly ILogger<UserSubmittedEmail> _logger;

    public UserSubmitEmailConsumer(IMediator mediator, ILogger<UserSubmittedEmail> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }
    
    public async Task Consume(ConsumeContext<UserSubmittedEmail> context)
    {
        await _mediator.Send(new ConfirmEmailCommand(context.Message.Email),
            context.CancellationToken);
        _logger.LogInformation($"Confirmation email: {context.Message.Email}");
    }
}