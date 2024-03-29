using MediatR;
using ProfileService.Domain.Shared;

namespace ProfileService.Application.Commands.CalculateUserRatingTimeCommand;

public class CalculateUserRatingTimeCommand : IRequest<Result>
{
    public string Email { get; set; }
    public int DurationMinites { get; set; }

    public CalculateUserRatingTimeCommand(string email, int durationMinites)
    {
        Email = email;
        DurationMinites = durationMinites;
    }
}