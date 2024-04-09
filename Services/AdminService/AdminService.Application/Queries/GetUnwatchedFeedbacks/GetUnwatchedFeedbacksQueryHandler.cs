using AdminService.Application.Models;
using AdminService.Domain.Models.FeedbackAggregate;
using AdminService.Domain.Models.FeedbackAggregate.Repos;
using DomainDriverDesignAbstractions;
using MediatR;

namespace AdminService.Application.Queries.GetUnwatchedFeedbacks;

public class GetUnwatchedFeedbacksQueryHandler
    : IRequestHandler<GetUnwatchedFeedbacksQuery, Result<List<FeedbackInformation>>>
{
    private readonly IFeedbackRepository _feedbackRepository;

    public GetUnwatchedFeedbacksQueryHandler(
        IFeedbackRepository feedbackRepository)
    {
        _feedbackRepository = feedbackRepository;
    }

    public async Task<Result<List<FeedbackInformation>>> Handle(GetUnwatchedFeedbacksQuery request, CancellationToken cancellationToken)
    {
        var response = await _feedbackRepository.GetFeedbacks(request.Count);

        return response.Select(x => new FeedbackInformation()
        {
            Id = x.Id.Value.ToString(),
            Email = x.EmailFrom.Value,
            Content = x.Content.Value,
            IsWatched = x.IsWatched
        }).ToList();
    }
}