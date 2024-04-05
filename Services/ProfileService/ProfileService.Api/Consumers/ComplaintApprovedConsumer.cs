using MassTransit;
using MassTransit.Contracts.ComplaintEvents;
using MediatR;
using ProfileService.Application.Commands.AddDoomSlayerPoint;

namespace ProfileService.Api.Consumers;

public class ComplaintApprovedConsumer : IConsumer<ComplaintApprovedByAdmin>
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<ComplaintApprovedConsumer> _logger;

    public ComplaintApprovedConsumer(
        IServiceScopeFactory scopeFactory,
        ILogger<ComplaintApprovedConsumer> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }
    
    public async Task Consume(ConsumeContext<ComplaintApprovedByAdmin> context)
    {
        using var scope = _scopeFactory.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        var result = await mediator.Send(new AddDoomSlayerPointCommand()
        {
            Email = context.Message.SenderEmail
        });

        if (result.IsSuccess)
        {
            _logger.LogInformation(@"Successfully added doom slayer points to {@Email}",
                context.Message.SenderEmail);
        }
        else
        {
            _logger.LogInformation(@"Cant add to {@Email} doom slayer points {@Error}",
                context.Message.SenderEmail,
                result.Error);
            throw new ArgumentException($"Error to add doom slayer points to {context.Message.SenderEmail}");
        }
    }
}