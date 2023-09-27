using Microsoft.AspNetCore.Mvc;

namespace PrintRequest.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class RequestController : ControllerBase
{

    private readonly ILogger<RequestController> _logger;

    public RequestController(ILogger<RequestController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public string Get()
    {
        return "123";
    }
}