using DomainDriverDesignAbstractions;
using MediatR;

namespace AdminService.Application.Commands.AddComplaint;

public class AddComplaintCommand
    : IRequest<Result>
{
    public string Id { get; set; }
    public string Content { get; set; }
    public string SenderEmail { get; set; }
    public string PossibleViolatorEmail { get; set; }
    public string ComplaintType { get; set; }

    public AddComplaintCommand(
        string id,
        string content,
        string senderEmail,
        string possibleViolatorEmail,
        string complaintType)
    {
        Id = id;
        Content = content;
        SenderEmail = senderEmail;
        PossibleViolatorEmail = possibleViolatorEmail;
        ComplaintType = complaintType;
    }
}