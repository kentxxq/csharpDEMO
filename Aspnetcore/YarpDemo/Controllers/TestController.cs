using Microsoft.AspNetCore.Mvc;

namespace YarpDemo.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class TestController : ControllerBase
{
    private readonly ILogger<TestController> _logger;

    public TestController(ILogger<TestController> logger)
    {
        _logger = logger;
    }
    
    public int Get()
    {
        return 111;
    }
}