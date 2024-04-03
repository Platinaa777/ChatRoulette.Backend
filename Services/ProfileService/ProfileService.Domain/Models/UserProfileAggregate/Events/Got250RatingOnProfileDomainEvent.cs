using DomainDriverDesignAbstractions;

namespace ProfileService.Domain.Models.UserProfileAggregate.Events;

public record Got250RatingOnProfileDomainEvent(string ProfileId) : IDomainEvent;
