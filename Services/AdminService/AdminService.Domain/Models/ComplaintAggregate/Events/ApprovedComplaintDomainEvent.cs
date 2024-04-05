using DomainDriverDesignAbstractions;

namespace AdminService.Domain.Models.ComplaintAggregate.Events;

public record ApprovedComplaintDomainEvent(
    string ComplaintId,
    string SenderEmail,
    string ViolatorEmail,
    int MinutesToBan,
    string Type)
    : IDomainEvent;