using AdminService.Application.Models;
using DomainDriverDesignAbstractions;
using MediatR;

namespace AdminService.Application.Queries.GetUnhandledComplaints;

public class GetUnhandledComplaintQuery
    : IRequest<Result<List<ComplaintInformation>>>
{
    public int Count { get; set; }
}