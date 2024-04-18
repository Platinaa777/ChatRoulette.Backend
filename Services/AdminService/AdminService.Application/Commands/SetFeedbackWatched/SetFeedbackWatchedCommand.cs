using DomainDriverDesignAbstractions;
using MediatR;

namespace AdminService.Application.Commands.SetFeedbackWatched;

public class SetFeedbackWatchedCommand : IRequest<Result>
{
    public string Id { get; set; } = string.Empty;
}