using FluentValidation;

namespace AdminService.Application.Commands.RejectComplaint;

public class RejectComplaintCommandValidator
    : AbstractValidator<RejectComplaint.RejectComplaintCommand>
{
    public RejectComplaintCommandValidator()
    {
        RuleFor(x => x.ComplaintId).NotEmpty();
    }
}