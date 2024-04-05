using FluentValidation;

namespace AdminService.Application.Queries.GetUnwatchedFeedbacks;

public class GetUnwatchedFeedbacksQueryValidator
    : AbstractValidator<GetUnwatchedFeedbacksQuery>
{
    public GetUnwatchedFeedbacksQueryValidator()
    {
        RuleFor(x => x.Count).GreaterThan(0);
    }
}