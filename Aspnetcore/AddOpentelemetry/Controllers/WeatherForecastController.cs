using AddOpenTelemetry;
using Microsoft.AspNetCore.Mvc;

namespace AddOpenTelemetry.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[] {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;
    private readonly IDemoService _demoService;

    public WeatherForecastController(ILogger<WeatherForecastController> logger,IDemoService demoService)
    {
        _logger = logger;
        _demoService = demoService;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public async Task<IEnumerable<WeatherForecast>> Get()
    {
        var demoData = _demoService.GetData();
        _logger.LogInformation("请求数据:{DemoData}",demoData);
        var httpclient = new HttpClient();
        var data = await httpclient.GetAsync("https://test.kentxxq.com/ip");
        if (data.IsSuccessStatusCode)
        {
            _logger.LogInformation("请求成功");
        }
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
    }
}