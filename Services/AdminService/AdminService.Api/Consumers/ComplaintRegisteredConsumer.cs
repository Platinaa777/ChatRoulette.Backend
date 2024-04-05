using AdminService.Application.Commands.AddComplaint;
using MassTransit;
using MassTransit.Contracts.ComplaintEvents;
using MediatR;

namespace AdminService.Api.Consumers;

public class ComplaintRegisteredConsumer
    : IConsumer<ComplaintRegistered>
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<ComplaintRegisteredConsumer> _logger;

    public ComplaintRegisteredConsumer(
        IServiceScopeFactory scopeFactory,
        ILogger<ComplaintRegisteredConsumer> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<ComplaintRegistered> context)
    {
        using var scope = _scopeFactory.CreateScope();
        
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        
        _logger.LogInformation("{@Consumer} was accepted complaint {@Id}",
            nameof(ComplaintRegisteredConsumer),
            context.Message.Id);

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
        
        _logger.LogInformation("{@Consumer} was handled complaint {@Id}",
            nameof(ComplaintRegisteredConsumer),
            context.Message.Id);
    }
}