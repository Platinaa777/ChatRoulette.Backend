using FluentValidation;

namespace ProfileService.Application.Commands.CalculateUserRatingTimeCommand;

public class CalculateUserRatingTimeCommandValidator : AbstractValidator<CalculateUserRatingTimeCommand>
{
    public CalculateUserRatingTimeCommandValidator()
    {
        RuleFor(x => x.Email).EmailAddress();
        RuleFor(x => x.DurationMinites).GreaterThan(0);
    }   
}