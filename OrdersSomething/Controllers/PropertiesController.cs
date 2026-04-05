using MediatR;
using Microsoft.AspNetCore.Mvc;
using OrdersSomething.Features.Properties.Commands;
using OrdersSomething.Features.Properties.Queries;

namespace OrdersSomething.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PropertiesController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var query = new GetAllPropertiesQuery();
        var result = await mediator.Send(query);
        return Ok(result);
    }
    
    [HttpGet]
    [Route("{propertyId}")]
    public async Task<IActionResult> GetByPropertyId([FromRoute] Guid propertyId)
    {
        var query = new GetPropertyByIdQuery(propertyId);
        var result = await mediator.Send(query);
        return Ok(result);
    }
    
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