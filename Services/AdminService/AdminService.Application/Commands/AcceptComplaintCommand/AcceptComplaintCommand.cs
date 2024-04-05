using DomainDriverDesignAbstractions;
using MediatR;

namespace AdminService.Application.Commands.AcceptComplaintCommand;

public class AcceptComplaintCommand : IRequest<Result>
{
    public string ComplaintId { get; set; }
}