using FluentValidation;

namespace AdminService.Application.Queries.GetUnhandledComplaints;

public class GetUnhandledComplaintQueryValidator
    : AbstractValidator<GetUnhandledComplaintQuery>
{
    public GetUnhandledComplaintQueryValidator()
    {
        RuleFor(x => x.Count).GreaterThan(0);
    }
}