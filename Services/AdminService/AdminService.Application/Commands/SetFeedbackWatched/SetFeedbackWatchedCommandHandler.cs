using AdminService.Domain.Errors;
using AdminService.Domain.Models.FeedbackAggregate.Repos;
using AdminService.Domain.Models.Shared;
using DomainDriverDesignAbstractions;
using MediatR;

namespace AdminService.Application.Commands.SetFeedbackWatched;

public class SetFeedbackWatchedCommandHandler
    : IRequestHandler<SetFeedbackWatchedCommand, Result>
{
    private readonly IFeedbackRepository _feedbackRepository;
    private readonly IUnitOfWork _unitOfWork;

    public SetFeedbackWatchedCommandHandler(
        IFeedbackRepository feedbackRepository,
        IUnitOfWork unitOfWork)
    {
        _feedbackRepository = feedbackRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Result> Handle(SetFeedbackWatchedCommand request, CancellationToken cancellationToken)
    {
        var idResult = Id.Create(request.Id);
        if (idResult.IsFailure)
            return Result.Failure(FeedbackError.InvalidId);

        var feedback = await _feedbackRepository.FindById(idResult.Value);

        if (feedback is null)
            return Result.Failure(FeedbackError.NotFound);
        
        feedback.HandleByAdmin();

        await _feedbackRepository.UpdateFeedback(feedback);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}