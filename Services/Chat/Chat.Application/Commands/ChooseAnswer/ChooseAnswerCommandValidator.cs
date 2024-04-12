using FluentValidation;

namespace Chat.Application.Commands.ChooseAnswer;

public class ChooseAnswerCommandValidator : AbstractValidator<ChooseAnswerCommand>
{
    public ChooseAnswerCommandValidator()
    {
        RuleFor(x => x.Answer).NotEmpty();
        RuleFor(x => x.PlayerEmail).EmailAddress();
        RuleFor(x => x.RoundId).NotEmpty();
    }
}