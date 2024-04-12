using Chat.Application.Queries.GetWinner;
using FluentValidation;

namespace Chat.Application.Commands.DefineWinner;

public class DefineWinnerQueryValidator : AbstractValidator<DefineWinnerQuery>
{
    public DefineWinnerQueryValidator()
    {
        RuleFor(x => x.RoundId).NotEmpty();
    }
}