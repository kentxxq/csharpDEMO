using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace AddWebApiClient.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[] {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;
    private readonly IIpApi _ipApi;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, IIpApi ipApi)
    {
        _logger = logger;
        _ipApi = ipApi;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
    }
    
    /// <summary>
    /// 获取ip数据
    /// </summary>
    /// <returns>demo数据</returns>
    [HttpGet]
    public async Task<string> GetIpInfo(string ip)
    {
        var result = await _ipApi.GetIpInfoAsync(ip);
        var json = JsonSerializer.Serialize(result, new JsonSerializerOptions 
        { 
            WriteIndented = true ,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        });
        return json;
    }
}