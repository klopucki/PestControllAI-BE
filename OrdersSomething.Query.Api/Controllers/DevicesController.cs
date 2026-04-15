using MediatR;
using Microsoft.AspNetCore.Mvc;
using OrdersSomething.Query.Api.Features.DeviceEvents;
using OrdersSomething.Query.Api.Features.Devices;

namespace OrdersSomething.Query.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DevicesController(IMediator mediator) : ControllerBase
{
    [Route("{deviceId}")]
    [HttpGet]
    public async Task<IActionResult> GetById([FromRoute] Guid deviceId)
    {
        var query = new GetDevicesByIdQuery(deviceId);
        var result = await mediator.Send(query);
        return Ok(result);
    }
    
    [Route("property/{propertyId}")]
    [HttpGet]
    public async Task<IActionResult> GetByPropertyId([FromRoute] Guid propertyId)
    {
        var query = new GetDevicesByPropertyIdQuery(propertyId);
        var result = await mediator.Send(query);
        return Ok(result);
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