using DomainDriverDesignAbstractions;
using MediatR;

namespace AdminService.Application.Commands.AddFeedbackCommand;

public class AddFeedbackCommand
    : IRequest<Result>
{
    public string EmailFrom { get; set; }
    public string Content { get; set; }
}