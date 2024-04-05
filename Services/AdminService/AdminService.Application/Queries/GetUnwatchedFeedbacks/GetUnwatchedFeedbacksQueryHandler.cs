using AdminService.Domain.Models.FeedbackAggregate;
using AdminService.Domain.Models.FeedbackAggregate.Repos;
using MediatR;

namespace AdminService.Application.Queries.GetUnwatchedFeedbacks;

public class GetUnwatchedFeedbacksQueryHandler
    : IRequestHandler<GetUnwatchedFeedbacksQuery, List<Feedback>>
{
    private readonly IFeedbackRepository _feedbackRepository;

    public GetUnwatchedFeedbacksQueryHandler(
        IFeedbackRepository feedbackRepository)
    {
        _feedbackRepository = feedbackRepository;
    }

    public async Task<List<Feedback>> Handle(GetUnwatchedFeedbacksQuery request, CancellationToken cancellationToken)
    {
        return await _feedbackRepository.GetFeedbacks(request.Count);
    }
}