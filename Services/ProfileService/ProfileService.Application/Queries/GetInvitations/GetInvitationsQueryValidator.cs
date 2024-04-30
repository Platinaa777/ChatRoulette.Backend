using FluentValidation;
using ProfileService.Application.Constants;

namespace ProfileService.Application.Queries.GetInvitations;

public class GetInvitationsQueryValidator : AbstractValidator<GetInvitationsQuery>
{
    public GetInvitationsQueryValidator()
    {
        RuleFor(x => x.Email)
            .EmailAddress()
            .WithMessage(ValidationConstants.InvalidEmail);
    }   
}