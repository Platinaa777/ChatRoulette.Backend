using FluentValidation;

namespace AdminService.Application.Commands.AddFeedbackCommand;

public class AddFeedbackCommandValidator
    : AbstractValidator<AddFeedbackCommand>
{
    public AddFeedbackCommandValidator()
    {
        RuleFor(x => x.Content).MinimumLength(10);
        RuleFor(x => x.EmailFrom).EmailAddress();
    }
}