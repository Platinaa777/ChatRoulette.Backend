using FluentValidation;

namespace AdminService.Application.Commands.AddComplaint;

public class AddComplaintCommandValidator : AbstractValidator<AddComplaint.AddComplaintCommand>
{
    public AddComplaintCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Content).NotEmpty();
        RuleFor(x => x.SenderEmail).EmailAddress();
        RuleFor(x => x.PossibleViolatorEmail).EmailAddress();
        RuleFor(x => x.SenderEmail).NotEqual(x => x.PossibleViolatorEmail);
    }
}