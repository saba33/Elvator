using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TransportLink.Application.Shipments.Queries.GetShipmentById;

namespace TransportLink.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    private readonly ILogger<HealthController> _logger;
    private readonly ISender _sender;

    public HealthController(ILogger<HealthController> logger, ISender sender)
    {
        _logger = logger;
        _sender = sender;
    }

    [HttpGet("status")]
    [AllowAnonymous]
    public IActionResult GetStatus()
    {
        _logger.LogInformation("Health status requested at {Timestamp}", DateTimeOffset.UtcNow);
        return Ok(new
        {
            status = "Healthy",
            timestamp = DateTimeOffset.UtcNow
        });
    }

    [HttpGet("shipments/{id:guid}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetShipment(Guid id, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetShipmentByIdQuery(id), cancellationToken);
        if (result.IsFailure)
        {
            return NotFound(new ProblemDetails
            {
                Title = "Shipment not found",
                Detail = result.Error,
                Status = StatusCodes.Status404NotFound
            });
        }

        return Ok(result.Value);
    }
}
