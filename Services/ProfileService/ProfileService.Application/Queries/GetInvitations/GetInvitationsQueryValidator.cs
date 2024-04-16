using FluentValidation;

namespace ProfileService.Application.Queries.GetInvitations;

public class GetInvitationsQueryValidator : AbstractValidator<GetInvitationsQuery>
{
    public GetInvitationsQueryValidator()
    {
        RuleFor(x => x.Email).EmailAddress();
    }   
}