using DomainDriverDesignAbstractions;
using MediatR;

namespace AdminService.Application.Commands.AddComplaintCommand;

public class AddComplaintCommand
    : IRequest<Result>
{
    public string Id { get; set; }
    public string Content { get; set; }
    public string From { get; set; }
    public string PossibleIntruderEmail { get; set; }

    public AddComplaintCommand(string id, string content, string from, string possibleIntruderEmail)
    {
        Id = id;
        Content = content;
        From = from;
        PossibleIntruderEmail = possibleIntruderEmail;
    }
}