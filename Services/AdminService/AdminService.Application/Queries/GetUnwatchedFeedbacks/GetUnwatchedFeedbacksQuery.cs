using AdminService.Domain.Models.FeedbackAggregate;
using MediatR;

namespace AdminService.Application.Queries.GetUnwatchedFeedbacks;

public class GetUnwatchedFeedbacksQuery
    : IRequest<List<Feedback>>
{
    public int Count { get; set; }
}