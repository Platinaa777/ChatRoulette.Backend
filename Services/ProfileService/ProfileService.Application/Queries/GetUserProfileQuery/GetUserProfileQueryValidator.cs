using FluentValidation;

namespace ProfileService.Application.Queries.GetUserProfileQuery;

public class GetUserProfileQueryValidator : AbstractValidator<GetUserProfileQuery>
{
    public GetUserProfileQueryValidator()
    {
        RuleFor(x => x.Email).EmailAddress();
    }
}