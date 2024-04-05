using DomainDriverDesignAbstractions;
using MediatR;

namespace AdminService.Application.Commands.AcceptComplaint;

public class AcceptComplaintCommand : IRequest<Result>
{
    public string ComplaintId { get; set; }
    public int MinutesDuration { get; set; }
}