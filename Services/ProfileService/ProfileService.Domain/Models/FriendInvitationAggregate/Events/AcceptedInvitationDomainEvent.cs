using ProfileService.Domain.Shared;

namespace ProfileService.Domain.Models.FriendInvitationAggregate.Events;

public record AcceptedInvitationDomainEvent(string senderId, string receiverId) : IDomainEvent;
