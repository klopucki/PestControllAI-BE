using MediatR;
using Microsoft.AspNetCore.Mvc;
using OrdersSomething.Command.Api.Features.Devices.Commands;

namespace OrdersSomething.Command.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DevicesController(IMediator mediator) : ControllerBase
{
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
}