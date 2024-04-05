using FluentValidation;
using MediatR;

namespace AdminService.Application.Commands.RejectComplaintCommand;

public class RejectComplaintCommandValidator
    : AbstractValidator<RejectComplaintCommand>
{
    public RejectComplaintCommandValidator()
    {
        RuleFor(x => x.ComplaintId).NotEmpty();
    }
}