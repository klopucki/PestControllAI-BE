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
        await mediator.Send(command);
        return Ok();
    }

    [HttpPut]
    public async Task<IActionResult> Modify([FromBody] UpsertPropertyCommand command)
    {
        await mediator.Send(command);
        return Ok();
    }

    [HttpPatch]
    public async Task<IActionResult> UpdateStatus([FromBody] DeletePropertyCommand command)
    {
        await mediator.Send(command);
        return Ok();
    }
}