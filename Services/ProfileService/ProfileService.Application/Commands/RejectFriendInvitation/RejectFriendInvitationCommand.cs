using DomainDriverDesignAbstractions;
using MediatR;

namespace ProfileService.Application.Commands.RejectFriendInvitation;

public class RejectFriendInvitationCommand : IRequest<Result>
{
    public string AnswerEmail { get; set; }
    public string SenderEmail { get; set; }

    public RejectFriendInvitationCommand(string AnswerEmail, string SenderEmail)
    {
        this.AnswerEmail = AnswerEmail;
        this.SenderEmail = SenderEmail;
    }
}