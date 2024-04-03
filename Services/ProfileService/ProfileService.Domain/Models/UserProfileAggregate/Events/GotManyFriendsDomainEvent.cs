using DomainDriverDesignAbstractions;

namespace ProfileService.Domain.Models.UserProfileAggregate.Events;

public record GotManyFriendsDomainEvent(string ProfileId) : IDomainEvent;
