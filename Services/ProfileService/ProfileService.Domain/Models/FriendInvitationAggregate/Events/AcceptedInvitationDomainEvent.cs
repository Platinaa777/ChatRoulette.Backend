using DomainDriverDesignAbstractions;

namespace ProfileService.Domain.Models.FriendInvitationAggregate.Events;

public record AcceptedInvitationDomainEvent(string SenderId, string ReceiverId) : IDomainEvent;
