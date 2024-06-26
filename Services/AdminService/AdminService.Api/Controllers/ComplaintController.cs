using AdminService.Application.Commands.AcceptComplaint;
using AdminService.Application.Commands.AddComplaint;
using AdminService.Application.Commands.RejectComplaint;
using AdminService.Application.Models;
using AdminService.Application.Queries.GetUnhandledComplaints;
using AdminService.HttpModels.Requests;
using AdminService.HttpModels.Requests.Complaint;
using DomainDriverDesignAbstractions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AdminService.Api.Controllers;

[ApiController]
[Route($"[controller]")]
public class ComplaintController : ControllerBase
{
    private readonly IMediator _mediator;

    public ComplaintController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{count:int}")]
    public async Task<ActionResult<Result<List<ComplaintInformation>>>> GetUnhandledComplaints(int count)
    {
        var response = await _mediator.Send(new GetUnhandledComplaintQuery(count));

        if (response.IsFailure)
            return BadRequest(response);
        
        return Ok(response);
    }
    
    [HttpPost("add-complaint")]
    public async Task<ActionResult<Result>> AddComplaint([FromBody] ComplaintAdd request)
    {
        var response = await _mediator.Send(new AddComplaintCommand(
            Guid.NewGuid().ToString(),
            request.Content,
            request.SenderEmail,
            request.PossibleViolatorEmail,
            request.ComplaintType));

        if (response.IsFailure)
            return BadRequest(response);

        return Ok(response);
    }

    [HttpPut("accept")]
    public async Task<ActionResult<Result>> MarkComplaintAsAccepted([FromBody] AcceptComplaintRequest request)
    {
        var result = await _mediator.Send(new AcceptComplaintCommand()
        {
            ComplaintId = request.Id,
            MinutesDuration = request.DurationMinutes
        });

        if (result.IsFailure)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpPut("reject")]
    public async Task<ActionResult<Result>> MarkComplaintAsRejected([FromBody] RejectComplaintRequest request)
    {
        var result = await _mediator.Send(new RejectComplaintCommand()
        {
            ComplaintId = request.ComplaintId
        });

        if (result.IsFailure)
            return BadRequest(result);

        return Ok(result);
    }
}
