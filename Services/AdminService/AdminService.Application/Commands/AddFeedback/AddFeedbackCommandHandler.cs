using AdminService.Domain.Models.FeedbackAggregate;
using AdminService.Domain.Models.FeedbackAggregate.Repos;
using DomainDriverDesignAbstractions;
using MediatR;

namespace AdminService.Application.Commands.AddFeedback;

public class AddFeedbackCommandHandler
    : IRequestHandler<AddFeedback.AddFeedbackCommand, Result>
{
    private readonly IFeedbackRepository _feedbackRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddFeedbackCommandHandler(
        IFeedbackRepository feedbackRepository,
        IUnitOfWork unitOfWork)
    {
        _feedbackRepository = feedbackRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Result> Handle(AddFeedbackCommand request, CancellationToken cancellationToken)
    {
        var feedbackResult = Feedback.Create(
            id: Guid.NewGuid().ToString(),
            request.EmailFrom,
            request.Content,
            isWatched: false);
        
        if (feedbackResult.IsFailure)
            return Result.Failure(feedbackResult.Error);

        await _feedbackRepository.AddFeedback(feedbackResult.Value);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}