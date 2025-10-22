using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TransportLink.Application.Orders.Commands.CreateOrder;
using TransportLink.Application.Orders.DTOs;
using TransportLink.Application.Orders.Queries.GetOrderById;
using TransportLink.Domain.Common;

namespace TransportLink.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class OrdersController : ControllerBase
{
    private readonly ISender _sender;

    public OrdersController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    [ProducesResponseType(typeof(OrderDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateOrderCommand command, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(ToProblemDetails(result, StatusCodes.Status400BadRequest));
        }

        return CreatedAtAction(nameof(GetById), new { id = result.Value.Id }, result.Value);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(OrderDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetOrderByIdQuery(id), cancellationToken);

        if (result.IsFailure)
        {
            return NotFound(ToProblemDetails(result, StatusCodes.Status404NotFound));
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
