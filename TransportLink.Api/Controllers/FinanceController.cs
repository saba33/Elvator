using System;
using System.Collections.Generic;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TransportLink.Application.Auth.Authorization;
using TransportLink.Api.Contracts.Finance;
using TransportLink.Application.Finance.Commands.ConfigureCompanyPayroll;
using TransportLink.Application.Finance.Commands.SettleOrderPayment;
using TransportLink.Application.Finance.DTOs;
using TransportLink.Application.Finance.Queries.GetCompanyExpenses;
using TransportLink.Application.Finance.Queries.GetDriverPayouts;
using TransportLink.Domain.Common;

namespace TransportLink.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = AuthorizationPolicies.CompanyOnly)]
public sealed class FinanceController : ControllerBase
{
    private readonly ISender _sender;

    public FinanceController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost("company-payroll")]
    [ProducesResponseType(typeof(CompanyPayrollConfigDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ConfigureCompanyPayroll([FromBody] ConfigureCompanyPayrollRequest request, CancellationToken cancellationToken)
    {
        var command = new ConfigureCompanyPayrollCommand(
            request.CompanyId,
            request.SalaryMode,
            request.PerOrderRatePct,
            request.DailyRate,
            request.MonthlySalary);

        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure || result.Value is null)
        {
            return MapFailure(result);
        }

        return Ok(result.Value);
    }

    [HttpPost("orders/{orderId:guid}/settle")]
    [ProducesResponseType(typeof(IReadOnlyCollection<PaymentDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> SettleOrder(Guid orderId, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new SettleOrderPaymentCommand(orderId), cancellationToken);

        if (result.IsFailure || result.Value is null)
        {
            return MapFailure(result);
        }

        return Ok(result.Value);
    }

    [HttpGet("driver/{driverId:guid}/payouts")]
    [ProducesResponseType(typeof(IReadOnlyCollection<PaymentDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetDriverPayouts(Guid driverId, [FromQuery] DateTime? from, [FromQuery] DateTime? to, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetDriverPayoutsQuery(driverId, from, to), cancellationToken);

        if (result.IsFailure || result.Value is null)
        {
            return MapFailure(result);
        }

        return Ok(result.Value);
    }

    [HttpGet("company/{companyId:guid}/expenses")]
    [ProducesResponseType(typeof(IReadOnlyCollection<PaymentDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetCompanyExpenses(Guid companyId, [FromQuery] DateTime? from, [FromQuery] DateTime? to, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetCompanyExpensesQuery(companyId, from, to), cancellationToken);

        if (result.IsFailure || result.Value is null)
        {
            return MapFailure(result);
        }

        return Ok(result.Value);
    }

    private IActionResult MapFailure<T>(Result<T> result)
    {
        var statusCode = DetermineStatusCode(result.Error);
        return StatusCode(statusCode, ToProblemDetails(result, statusCode));
    }

    private static int DetermineStatusCode(string? error)
    {
        if (string.IsNullOrWhiteSpace(error))
        {
            return StatusCodes.Status400BadRequest;
        }

        return error.Contains("not found", StringComparison.OrdinalIgnoreCase)
            ? StatusCodes.Status404NotFound
            : StatusCodes.Status400BadRequest;
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
