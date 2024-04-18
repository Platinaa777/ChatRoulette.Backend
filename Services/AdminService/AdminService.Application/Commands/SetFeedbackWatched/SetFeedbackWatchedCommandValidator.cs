using FluentValidation;

namespace AdminService.Application.Commands.SetFeedbackWatched;

public class SetFeedbackWatchedCommandValidator
    : AbstractValidator<SetFeedbackWatchedCommand>
{
    public SetFeedbackWatchedCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}