using MediatR;
using Microsoft.AspNetCore.Mvc;
using OrdersSomething.Query.Api.Features.Properties;

namespace OrdersSomething.Query.Api.Controllers;

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
}