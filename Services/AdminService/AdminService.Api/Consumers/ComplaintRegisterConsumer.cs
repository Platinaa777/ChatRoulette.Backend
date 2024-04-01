using AdminService.Application.Commands.AddComplaintCommand;
using MassTransit;
using MassTransit.Contracts.ComplaintEvents;
using MediatR;

namespace AdminService.Api.Consumers;

public class ComplaintRegisterConsumer
    : IConsumer<ComplaintRegistered>
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<ComplaintRegistered> _logger;

    public ComplaintRegisterConsumer(
        IServiceScopeFactory scopeFactory,
        ILogger<ComplaintRegistered> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<ComplaintRegistered> context)
    {
        using var scope = _scopeFactory.CreateScope();
        
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        var result = await mediator.Send(new AddComplaintCommand(
            context.Message.Id,
            context.Message.Content,
            context.Message.SenderEmail,
            context.Message.PossibleViolatorEmail,
            context.Message.ComplaintType));

        if (result.IsFailure)
        {
            throw new ArgumentException("Message was unhandled");
        }
    }
}