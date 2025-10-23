using System;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using TransportLink.Application.Auth.Authorization;
using TransportLink.Api.Contracts.Drivers;
using TransportLink.Application.Drivers.Commands.UpdateDriverStatus;
using TransportLink.Application.Drivers.DTOs;
using TransportLink.Application.Drivers.Queries.GetAvailableDrivers;
using TransportLink.Domain.Common;

namespace TransportLink.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class DriversController : ControllerBase
{
    private readonly ISender _sender;

    public DriversController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet("available")]
    [Authorize(Policy = AuthorizationPolicies.CompanyOnly)]
    [ProducesResponseType(typeof(IReadOnlyCollection<DriverDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAvailableDrivers(CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetAvailableDriversQuery(), cancellationToken);
        if (result.IsFailure)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ToProblemDetails(result, StatusCodes.Status500InternalServerError));
        }

        return Ok(result.Value);
    }

    [HttpPatch("{driverId:guid}/status")]
    [Authorize(Policy = AuthorizationPolicies.CompanyOnly)]
    [ProducesResponseType(typeof(DriverDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateStatus(Guid driverId, [FromBody] UpdateDriverStatusRequest request, CancellationToken cancellationToken)
    {
        if (request is null)
        {
            return StatusCode(StatusCodes.Status400BadRequest, new ProblemDetails
            {
                Title = "Invalid request",
                Detail = "Request body is required.",
                Status = StatusCodes.Status400BadRequest
            });
        }

        var command = new UpdateDriverStatusCommand(driverId, request.IsAvailable);
        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return StatusCode(StatusCodes.Status404NotFound, ToProblemDetails(result, StatusCodes.Status404NotFound));
        }

        return Ok(result.Value);
    }

    [HttpPatch("self/status")]
    [Authorize(Policy = AuthorizationPolicies.DriverOnly)]
    [ProducesResponseType(typeof(DriverDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateSelfStatus([FromBody] UpdateDriverStatusRequest request, CancellationToken cancellationToken)
    {
        if (request is null)
        {
            return StatusCode(StatusCodes.Status400BadRequest, new ProblemDetails
            {
                Title = "Invalid request",
                Detail = "Request body is required.",
                Status = StatusCodes.Status400BadRequest
            });
        }

        var driverIdClaim = User.FindFirst(CustomClaimTypes.DriverId)?.Value;
        if (string.IsNullOrWhiteSpace(driverIdClaim) || !Guid.TryParse(driverIdClaim, out var driverId))
        {
            return Forbid();
        }

        var command = new UpdateDriverStatusCommand(driverId, request.IsAvailable);
        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            var status = result.Error?.Contains("not found", StringComparison.OrdinalIgnoreCase) == true
                ? StatusCodes.Status404NotFound
                : StatusCodes.Status400BadRequest;
            return StatusCode(status, ToProblemDetails(result, status));
        }

        return Ok(result.Value);
    }

    private static ProblemDetails ToProblemDetails<T>(Result<T> result, int statusCode)
    {
        return new ProblemDetails
        {
            Title = "Request failed",
            Detail = result.Error,
            Status = statusCode
        };
    }
}
