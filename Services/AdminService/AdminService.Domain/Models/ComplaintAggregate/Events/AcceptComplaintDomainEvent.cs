using DomainDriverDesignAbstractions;

namespace AdminService.Domain.Models.ComplaintAggregate.Events;

public record AcceptComplaintDomainEvent(string SenderEmail) : IDomainEvent;