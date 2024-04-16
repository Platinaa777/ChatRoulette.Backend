using FluentValidation;

namespace ProfileService.Application.Queries.GetPeersInformation;

public class GetPeersInformationQueryValidator
    : AbstractValidator<GetPeersInformationQuery>
{
    public GetPeersInformationQueryValidator()
    {
        RuleFor(x => x.PeerEmails).NotNull().NotEmpty();
    }
}