using DomainDriverDesignAbstractions;
using MediatR;

namespace AdminService.Application.Commands.RejectComplaint;

public class RejectComplaintCommand : IRequest<Result>
{
    public string ComplaintId { get; set; }
}