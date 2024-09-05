using AddSwaggerWithJWT.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AddSwaggerWithJWT.Controllers;

/// <inheritdoc />
[ApiExplorerSettings(GroupName = "v1")]
[ApiController]
[Route("[controller]/[action]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[] {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;
    private readonly JWTService _jwtService;

    /// <inheritdoc />
    public WeatherForecastController(ILogger<WeatherForecastController> logger, JWTService jwtService)
    {
        _logger = logger;
        _jwtService = jwtService;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast {
                Date = DateOnly.FromDateTime(DateTimeOffset.Now.AddDays(index).DateTime),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
    }

    /// <summary>
    /// 获取token
    /// </summary>
    /// <param name="uid">用户id</param>
    /// <param name="username">用户名</param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpGet]
    public ResultModel<string> GetToken(int uid, string username)
    {
        var token = _jwtService.GetToken(uid, username);
        return ResultModel<string>.Ok(token);
    }
}
