using DomainDriverDesignAbstractions;
using MediatR;

namespace AdminService.Application.Commands.RejectComplaintCommand;

public class RejectComplaintCommand : IRequest<Result>
{
    public string ComplaintId { get; set; }
}