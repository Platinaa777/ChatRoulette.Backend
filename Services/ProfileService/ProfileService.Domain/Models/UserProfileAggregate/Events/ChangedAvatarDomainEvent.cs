using DomainDriverDesignAbstractions;

namespace ProfileService.Domain.Models.UserProfileAggregate.Events;

public record ChangedAvatarDomainEvent(string UserId) : IDomainEvent;
