using DomainDriverDesignAbstractions;

namespace ProfileService.Domain.Models.UserHistoryAggregate.Events;

public record ReceivedAvatarAchievementDomainEvent(string UserId) : IDomainEvent;
