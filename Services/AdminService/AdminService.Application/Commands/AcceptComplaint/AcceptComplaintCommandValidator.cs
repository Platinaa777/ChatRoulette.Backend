using FluentValidation;

namespace AdminService.Application.Commands.AcceptComplaint;

public class AcceptComplaintCommandValidator
    : AbstractValidator<AcceptComplaint.AcceptComplaintCommand>
{
    public AcceptComplaintCommandValidator()
    {
        RuleFor(x => x.ComplaintId).NotEmpty();
        RuleFor(x => x.MinutesDuration).GreaterThan(0);
    }
}