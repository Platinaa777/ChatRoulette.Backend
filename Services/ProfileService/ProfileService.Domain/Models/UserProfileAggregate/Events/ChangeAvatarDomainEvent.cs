using DomainDriverDesignAbstractions;

namespace ProfileService.Domain.Models.UserProfileAggregate.Events;

public record ChangeAvatarDomainEvent(string UserId) : IDomainEvent;
