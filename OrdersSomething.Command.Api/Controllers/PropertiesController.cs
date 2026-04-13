using MediatR;
using Microsoft.AspNetCore.Mvc;
using OrdersSomething.Command.Api.Features.Properties.Commands;

namespace OrdersSomething.Command.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PropertiesController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] UpsertPropertyCommand command)
    {
        var upsertPropertyResponse = await mediator.Send(command);
        return Ok(upsertPropertyResponse);
    }

    [HttpPut]
    public async Task<IActionResult> Modify([FromBody] UpsertPropertyCommand command)
    {
        var upsertPropertyResponse = await mediator.Send(command);
        return Ok(upsertPropertyResponse);
    }

    [HttpPatch]
    public async Task<IActionResult> ModifyStatus([FromBody] DeletePropertyCommand command)
    {
        await mediator.Send(command);
        return Ok();
    }
}