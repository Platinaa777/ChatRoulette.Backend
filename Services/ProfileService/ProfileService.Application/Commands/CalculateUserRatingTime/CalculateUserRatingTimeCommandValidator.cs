using FluentValidation;

namespace ProfileService.Application.Commands.CalculateUserRatingTime;

public class CalculateUserRatingTimeCommandValidator : AbstractValidator<CalculateUserRatingTime.CalculateUserRatingTimeCommand>
{
    public CalculateUserRatingTimeCommandValidator()
    {
        RuleFor(x => x.Email).EmailAddress();
        RuleFor(x => x.DurationMinites).GreaterThan(0);
    }   
}