using AdminService.Application.Commands.AddComplaintCommand;
using AdminService.Application.Models;
using AdminService.Application.Queries.GetUnhandledComplaints;
using DomainDriverDesignAbstractions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AdminService.Api.Controllers;

[ApiController]
[Route($"[controller]")]
public class AdminController : ControllerBase
{
    private readonly IMediator _mediator;

    public AdminController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("complaint/{count:int}")]
    public async Task<ActionResult<Result<List<ComplaintInformation>>>> GetUnhandledComplaints(int count)
    {
        var response = await _mediator.Send(new GetUnhandledComplaintQuery(count));

        if (response.IsFailure)
            return BadRequest(response);
        
        return Ok(response);
    }
    
    [HttpPost("add-complaint")]
    public async Task<ActionResult<Result>> AddComplaintForTests([FromBody] ComplaintRequestAdd request)
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
}


public class ComplaintRequestAdd
{
    public string Content { get; set; }
    public string SenderEmail { get; set; }
    public string PossibleViolatorEmail { get; set; }
    public string ComplaintType { get; set; }
}