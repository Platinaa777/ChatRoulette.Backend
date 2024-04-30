using AuthService.Application.Constants;
using FluentValidation;

namespace AuthService.Application.Commands.GenerateToken;

public class GenerateTokenCommandValidator : AbstractValidator<GenerateTokenCommand>
{
    public GenerateTokenCommandValidator()
    {
        RuleFor(x => x.RefreshToken)
            .NotEmpty()
            .WithMessage(ValidationConstants.EmptyRefreshToken);
    }
}