using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TransportLink.Api.Contracts.Auth;
using TransportLink.Application.Auth.Commands.Login;
using TransportLink.Application.Auth.Commands.RefreshToken;
using TransportLink.Application.Auth.Commands.RegisterCompanyManager;
using TransportLink.Application.Auth.Commands.RegisterDriver;
using TransportLink.Application.Auth.DTOs;
using TransportLink.Domain.Common;

namespace TransportLink.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class AuthController : ControllerBase
{
    private readonly ISender _sender;

    public AuthController(ISender sender)
    {
        _sender = sender;
    }

    [AllowAnonymous]
    [HttpPost("register-company")]
    [ProducesResponseType(typeof(AuthTokensDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RegisterCompany([FromBody] RegisterCompanyRequest request, CancellationToken cancellationToken)
    {
        var command = new RegisterCompanyManagerCommand(request.CompanyId, request.Email, request.Password);
        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure || result.Value is null)
        {
            return BadRequest(ToProblemDetails(result, StatusCodes.Status400BadRequest));
        }

        return Ok(result.Value);
    }

    [AllowAnonymous]
    [HttpPost("register-driver")]
    [ProducesResponseType(typeof(AuthTokensDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RegisterDriver([FromBody] RegisterDriverRequest request, CancellationToken cancellationToken)
    {
        var command = new RegisterDriverCommand(request.DriverId, request.Email, request.Password);
        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure || result.Value is null)
        {
            return BadRequest(ToProblemDetails(result, StatusCodes.Status400BadRequest));
        }

        return Ok(result.Value);
    }

    [AllowAnonymous]
    [HttpPost("login")]
    [ProducesResponseType(typeof(AuthTokensDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        var command = new LoginCommand(request.Email, request.Password);
        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure || result.Value is null)
        {
            return Unauthorized(ToProblemDetails(result, StatusCodes.Status401Unauthorized));
        }

        return Ok(result.Value);
    }

    [AllowAnonymous]
    [HttpPost("refresh")]
    [ProducesResponseType(typeof(AuthTokensDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        var command = new RefreshTokenCommand(request.RefreshToken);
        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure || result.Value is null)
        {
            return BadRequest(ToProblemDetails(result, StatusCodes.Status400BadRequest));
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
