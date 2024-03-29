using MassTransit;
using MassTransit.Contracts.UserEvents;
using MediatR;
using ProfileService.Application.Commands.CalculateUserRatingTimeCommand;

namespace ProfileService.Api.Consumers;

public class UserRatingCalculatorConsumer : IConsumer<UserWasTalked>
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<UserSubmittedEmail> _logger;

    public UserRatingCalculatorConsumer(
        IServiceScopeFactory scopeFactory,
        ILogger<UserSubmittedEmail> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<UserWasTalked> context)
    {
        using (var scope = _scopeFactory.CreateScope())
        {
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            
            var result = await mediator.Send(
                new CalculateUserRatingTimeCommand(
                    context.Message.Email,
                    context.Message.Minites),
                context.CancellationToken);

            if (result.IsFailure)
            {
                _logger.LogInformation(
                    "System could not calculate user rating, user: {@Email}", 
                    context.Message.Email);
                return;
            }
            
            _logger.LogInformation(
                "Successfully added rating to user: {@Email}", 
                context.Message.Email);
        }
    }
}