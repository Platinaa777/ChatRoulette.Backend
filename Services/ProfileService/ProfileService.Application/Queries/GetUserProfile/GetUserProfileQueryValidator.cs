using FluentValidation;
using ProfileService.Application.Constants;

namespace ProfileService.Application.Queries.GetUserProfile;

public class GetUserProfileQueryValidator : AbstractValidator<GetUserProfileQuery>
{
    public GetUserProfileQueryValidator()
    {
        RuleFor(x => x.Email)
            .EmailAddress()
            .WithMessage(ValidationConstants.InvalidEmail);
    }
}