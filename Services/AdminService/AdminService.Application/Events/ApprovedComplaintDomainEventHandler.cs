using AdminService.Domain.Models.ComplaintAggregate.Events;
using MassTransit.Client.EventBus;
using MassTransit.Contracts.ComplaintEvents;
using MediatR;

namespace AdminService.Application.Events;

public class ApprovedComplaintDomainEventHandler
    : INotificationHandler<ApprovedComplaintDomainEvent>
{
    private readonly IEventBusClient _busClient;

    public ApprovedComplaintDomainEventHandler(IEventBusClient busClient)
    {
        _busClient = busClient;
    }
    
    public async Task Handle(ApprovedComplaintDomainEvent notification, CancellationToken cancellationToken)
    {
        var busCommand = new ComplaintApprovedByAdmin()
        {
            Id = notification.ComplaintId,
            ViolatorEmail = notification.ViolatorEmail,
            MinutesToBan = notification.MinutesToBan,
            Type = notification.Type
        };

        await _busClient.PublishAsync(busCommand, cancellationToken);
    }
}