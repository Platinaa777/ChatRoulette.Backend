using FluentValidation;
using ProfileService.Application.Constants;

namespace ProfileService.Application.Queries.GetPeersInformation;

public class GetPeersInformationQueryValidator
    : AbstractValidator<GetPeersInformationQuery>
{
    public GetPeersInformationQueryValidator()
    {
        RuleFor(x => x.PeerEmails).NotNull().NotEmpty()
            .WithMessage(ValidationConstants.PeerListEmpty);
    }
}