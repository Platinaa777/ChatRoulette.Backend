using FluentValidation;
using ProfileService.Application.Constants;

namespace ProfileService.Application.Commands.CalculateUserRatingTime;

public class CalculateUserRatingTimeCommandValidator : AbstractValidator<CalculateUserRatingTimeCommand>
{
    public CalculateUserRatingTimeCommandValidator()
    {
        RuleFor(x => x.Email)
            .EmailAddress()
            .WithMessage(ValidationConstants.InvalidEmail);
        RuleFor(x => x.DurationMinites)
            .GreaterThan(0)
            .WithMessage(ValidationConstants.RatingPointShouldBePositive);
    }   
}