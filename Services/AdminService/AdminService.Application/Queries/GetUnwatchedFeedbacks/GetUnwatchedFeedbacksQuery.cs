using AdminService.Application.Models;
using AdminService.Domain.Models.FeedbackAggregate;
using DomainDriverDesignAbstractions;
using MediatR;

namespace AdminService.Application.Queries.GetUnwatchedFeedbacks;

public class GetUnwatchedFeedbacksQuery
    : IRequest<Result<List<FeedbackInformation>>>
{
    public int Count { get; set; }
}