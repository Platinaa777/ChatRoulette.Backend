using FluentValidation;

namespace AdminService.Application.Commands.AddFeedback;

public class AddFeedbackCommandValidator
    : AbstractValidator<AddFeedback.AddFeedbackCommand>
{
    public AddFeedbackCommandValidator()
    {
        RuleFor(x => x.Content).MinimumLength(10);
        RuleFor(x => x.EmailFrom).EmailAddress();
    }
}