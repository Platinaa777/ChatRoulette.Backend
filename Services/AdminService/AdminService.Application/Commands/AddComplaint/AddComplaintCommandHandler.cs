using AdminService.Domain.Models.ComplaintAggregate;
using AdminService.Domain.Models.ComplaintAggregate.Repos;
using DomainDriverDesignAbstractions;
using MediatR;

namespace AdminService.Application.Commands.AddComplaint;

public class AddComplaintCommandHandler :
    IRequestHandler<AddComplaint.AddComplaintCommand, Result>
{
    private readonly IComplaintRepository _complaintRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddComplaintCommandHandler(
        IComplaintRepository complaintRepository,
        IUnitOfWork unitOfWork)
    {
        _complaintRepository = complaintRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Result> Handle(AddComplaint.AddComplaintCommand request, CancellationToken cancellationToken)
    {
        var complaintResult = Complaint.Create(
            request.Id,
            request.Content,
            request.SenderEmail,
            request.PossibleViolatorEmail,
            request.ComplaintType,
            isHandled: false);
        
        if (complaintResult.IsFailure)
            return Result.Failure(complaintResult.Error);

        await _complaintRepository.AddComplaint(complaintResult.Value);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Result.Success();
    }
}