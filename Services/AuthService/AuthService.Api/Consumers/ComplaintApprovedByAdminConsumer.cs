using AuthService.Application.Commands.BanUser;
using MassTransit;
using MassTransit.Contracts.ComplaintEvents;
using MediatR;

namespace AuthService.Api.Consumers;

public class ComplaintApprovedByAdminConsumer : IConsumer<ComplaintApprovedByAdmin>
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<ComplaintApprovedByAdminConsumer> _logger;

    public ComplaintApprovedByAdminConsumer(
        IServiceScopeFactory scopeFactory,
        ILogger<ComplaintApprovedByAdminConsumer> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<ComplaintApprovedByAdmin> context)
    {
        using var scope = _scopeFactory.CreateScope();
        
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        
        _logger.LogInformation("{@Consumer} was accepted complaint {@Id}",
            nameof(ComplaintApprovedByAdminConsumer),
            context.Message.Id);

        var result = await mediator.Send(new BanUserCommand()
        {
            ViolatorEmail = context.Message.ViolatorEmail,
            MinutesToBan = context.Message.MinutesToBan
        });

        if (result.IsFailure)
        {
            throw new ArgumentException("Message was unhandled");
        }
        
        _logger.LogInformation("{@Consumer} was handled complaint {@Id}",
            nameof(ComplaintApprovedByAdminConsumer),
            context.Message.Id);
    }
}