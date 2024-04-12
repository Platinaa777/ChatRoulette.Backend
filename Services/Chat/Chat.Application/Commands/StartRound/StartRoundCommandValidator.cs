using FluentValidation;

namespace Chat.Application.Commands.StartRound;

public class StartRoundCommandValidator
     : AbstractValidator<StartRoundCommand>
{
     public StartRoundCommandValidator()
     {
          RuleFor(x => x.FirstEmail).EmailAddress();
          RuleFor(x => x.SecondEmail).EmailAddress();
     }
}