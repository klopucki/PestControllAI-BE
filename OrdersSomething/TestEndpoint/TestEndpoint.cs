using Microsoft.AspNetCore.Mvc;

namespace OrdersSomething.TestEndpoint;

[ApiController]
[Route("[controller]")]
public class TestEndpoint : ControllerBase
{

    [HttpGet(Name = "xd")]
    public String Get()
    {
        return "Sraka";
    }
}