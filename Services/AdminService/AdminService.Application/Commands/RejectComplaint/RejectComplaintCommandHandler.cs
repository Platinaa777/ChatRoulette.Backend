using AdminService.Domain.Errors;
using AdminService.Domain.Models.ComplaintAggregate.Repos;
using AdminService.Domain.Models.Shared;
using DomainDriverDesignAbstractions;
using MediatR;

namespace AdminService.Application.Commands.RejectComplaint;

public class RejectComplaintCommandHandler
    : IRequestHandler<RejectComplaint.RejectComplaintCommand, Result>
{
    private readonly IComplaintRepository _complaintRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RejectComplaintCommandHandler(
        IComplaintRepository complaintRepository,
        IUnitOfWork unitOfWork)
    {
        _complaintRepository = complaintRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Result> Handle(RejectComplaint.RejectComplaintCommand request, CancellationToken cancellationToken)
    {
        var idResult = Id.Create(request.ComplaintId);
        if (idResult.IsFailure)
            return Result.Failure(idResult.Error);

        var complaint = await _complaintRepository.FindComplaintById(idResult.Value);
        if (complaint is null)
            return Result.Failure(ComplaintError.ComplaintNotFound);
        
        if (complaint.IsHandledByAdmin())
            return Result.Failure(ComplaintError.AlreadyHandled);
        
        complaint.SetRejected();

        if (!await _complaintRepository.UpdateComplaint(complaint))
        {
            return Result.Failure(ComplaintError.CantUpdateComplaint);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Result.Success();
    }
}