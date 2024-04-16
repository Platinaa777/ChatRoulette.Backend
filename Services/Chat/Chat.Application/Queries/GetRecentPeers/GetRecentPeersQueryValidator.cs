using FluentValidation;

namespace Chat.Application.Queries.GetRecentPeers;

public class GetRecentPeersQueryValidator
    : AbstractValidator<GetRecentPeersQuery>
{
    public GetRecentPeersQueryValidator()
    {
        RuleFor(x => x.Email).EmailAddress();
    }
}