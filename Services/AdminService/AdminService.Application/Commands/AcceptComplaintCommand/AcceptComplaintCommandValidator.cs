using FluentValidation;

namespace AdminService.Application.Commands.AcceptComplaintCommand;

public class AcceptComplaintCommandValidator
    : AbstractValidator<AcceptComplaintCommand>
{
    public AcceptComplaintCommandValidator()
    {
        RuleFor(x => x.ComplaintId).NotEmpty();
    }
}