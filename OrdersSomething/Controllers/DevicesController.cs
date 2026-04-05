using MediatR;
using Microsoft.AspNetCore.Mvc;
using OrdersSomething.Features.DeviceEvents.Queries;
using OrdersSomething.Features.Devices.Commands;
using OrdersSomething.Features.Devices.Query;

namespace OrdersSomething.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DevicesController(IMediator mediator) : ControllerBase
{
    [Route("property/{propertyId}")]
    [HttpGet]
    public async Task<IActionResult> GetByPropertyId([FromRoute] Guid propertyId)
    {
        var query = new GetDevicesByPropertyIdQuery(propertyId);
        var result = await mediator.Send(query);
        return Ok(result);
    }

    [HttpPatch]
    public async Task<IActionResult> DeleteDevice([FromBody] DeleteDeviceCommand command)
    {
        await mediator.Send(command);
        return Ok();
    }

    [HttpPatch]
    [Route("listening")]
    public async Task<IActionResult> UpdateListeningDevice([FromBody] UpdateListeningCommand command)
    {
        await mediator.Send(command);
        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] UpsertDeviceCommand command)
    {
        await mediator.Send(command);
        return Ok();
    }

    [HttpPut]
    public async Task<IActionResult> Modify([FromBody] UpsertDeviceCommand command)
    {
        await mediator.Send(command);
        return Ok();
    }

    [HttpGet]
    [Route("{deviceId}/events")]
    public async Task<IActionResult> GetEventsByDeviceId([FromRoute] Guid deviceId)
    {
        var query = new GetDeviceEventsByDeviceIdQuery(deviceId);
        var result = await mediator.Send(query);
        return Ok(result);
    }
}