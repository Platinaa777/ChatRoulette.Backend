using DomainDriverDesignAbstractions;
using MediatR;

namespace ProfileService.Application.Commands.AcceptFriendInvitation;

public class AcceptFriendInvitationCommand : IRequest<Result>
{
    public string AnswerEmail { get; set; }
    public string SenderEmail { get; set; }

    public AcceptFriendInvitationCommand(string AnswerEmail, string SenderEmail)
    {
        this.AnswerEmail = AnswerEmail;
        this.SenderEmail = SenderEmail;
    }
}