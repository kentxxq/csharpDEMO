using Microsoft.AspNetCore.Mvc;

namespace ConfigUsage.Controllers;

[ApiController]
[Route("[controller]")]
public class ConfigController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<ConfigController> _logger;

    public ConfigController(ILogger<ConfigController> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }

    [HttpGet(Name = "PrintConfig")]
    public string Get()
    {
        _logger.LogInformation("1");
        return "1";
    }
}
