using System;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TransportLink.Application.Auth.Authorization;
using TransportLink.Application.Matching.Commands.AssignDriverToOrder;
using TransportLink.Application.Orders.DTOs;
using TransportLink.Domain.Common;

namespace TransportLink.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = AuthorizationPolicies.CompanyOnly)]
public sealed class MatchingController : ControllerBase
{
    private readonly ISender _sender;

    public MatchingController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost("assign")]
    [ProducesResponseType(typeof(OrderDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Assign([FromBody] AssignDriverToOrderCommand command, CancellationToken cancellationToken)
    {
        if (command is null)
        {
            return StatusCode(StatusCodes.Status400BadRequest, new ProblemDetails
            {
                Title = "Invalid request",
                Detail = "Request body is required.",
                Status = StatusCodes.Status400BadRequest
            });
        }

        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            var status = result.Error.Contains("not found", StringComparison.OrdinalIgnoreCase)
                ? StatusCodes.Status404NotFound
                : StatusCodes.Status400BadRequest;
            return StatusCode(status, ToProblemDetails(result, status));
        }

        return Ok(result.Value);
    }

    private ProblemDetails ToProblemDetails<T>(Result<T> result, int statusCode)
    {
        return new ProblemDetails
        {
            Title = "Request failed",
            Detail = result.Error,
            Status = statusCode
        };
    }
}
