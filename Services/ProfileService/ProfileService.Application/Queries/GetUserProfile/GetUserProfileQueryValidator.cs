using FluentValidation;

namespace ProfileService.Application.Queries.GetUserProfile;

public class GetUserProfileQueryValidator : AbstractValidator<GetUserProfile.GetUserProfileQuery>
{
    public GetUserProfileQueryValidator()
    {
        RuleFor(x => x.Email).EmailAddress();
    }
}