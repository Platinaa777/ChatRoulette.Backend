using AdminService.Application.Commands.AddFeedback;
using AdminService.Application.Commands.SetFeedbackWatched;
using AdminService.Application.Models;
using AdminService.Application.Queries.GetUnwatchedFeedbacks;
using AdminService.Domain.Models.FeedbackAggregate;
using AdminService.HttpModels.Requests.Feedback;
using DomainDriverDesignAbstractions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AdminService.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class FeedbackController : ControllerBase
{
    private readonly IMediator _mediator;

    public FeedbackController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("add")]
    public async Task<ActionResult<Result>> AddFeedback([FromBody] SendFeedbackRequest request)
    {
        var result = await _mediator.Send(new AddFeedbackCommand()
        {
            EmailFrom = request.FromEmail,
            Content = request.Content
        });

        if (result.IsFailure)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpGet("{count:int}")]
    public async Task<ActionResult<List<FeedbackInformation>>> GetFeedbacks(int count)
    {
        return Ok(await _mediator.Send(new GetUnwatchedFeedbacksQuery() { Count = count }));
    }

    [HttpPost("{complaintId}")]
    public async Task<ActionResult<Result>> MarkFeedbackAsHandled(string complaintId)
    {
        var result = await _mediator.Send(new SetFeedbackWatchedCommand() { Id = complaintId });

        if (result.IsFailure)
            return BadRequest(result);

        return Ok(result);
    }
}