using AdminService.Application.Models;
using AdminService.Domain.Errors;
using AdminService.Domain.Models.ComplaintAggregate.Repos;
using DomainDriverDesignAbstractions;
using MediatR;

namespace AdminService.Application.Queries.GetUnhandledComplaints;

public class GetUnhandledComplaintQueryHandler 
    : IRequestHandler<GetUnhandledComplaintQuery, Result<List<ComplaintInformation>>>
{
    private readonly IComplaintRepository _complaintRepository;

    public GetUnhandledComplaintQueryHandler(
        IComplaintRepository complaintRepository)
    {
        _complaintRepository = complaintRepository;
    }
    
    public async Task<Result<List<ComplaintInformation>>> Handle(GetUnhandledComplaintQuery request, CancellationToken cancellationToken)
    {
        var complaints = await _complaintRepository.GetComplaints(request.Count);

        if (complaints.Count == 0)
            return Result.Failure<List<ComplaintInformation>>(ComplaintError.NotFoundAnyComplaints);

        var complaintInformationList = new List<ComplaintInformation>();
        foreach (var complaint in complaints)
        {
            complaintInformationList.Add(new ComplaintInformation()
            {
                Id = complaint.Id.Value.ToString(),
                Content = complaint.Content.Value,
                SenderEmail = complaint.SenderEmail.Value,
                ViolatorEmail = complaint.ViolatorEmail.Value,
                ComplaintType = complaint.ComplaintType.Name
            });
        }

        return Result.Success(complaintInformationList);
    }
}