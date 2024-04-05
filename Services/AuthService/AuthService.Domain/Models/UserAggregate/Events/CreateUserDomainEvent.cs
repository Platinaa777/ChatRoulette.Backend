using DomainDriverDesignAbstractions;

namespace AuthService.Domain.Models.UserAggregate.Events;

public record CreateUserDomainEvent(string UserId) : IDomainEvent;
