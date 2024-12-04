using AddOpenAPIWithJWT.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AddOpenAPIWithJWT.Controllers;

/// <inheritdoc />
[ApiExplorerSettings(GroupName = "v1")]
[ApiController]
[Route("[controller]/[action]")]
public class UserController : ControllerBase
{
    private static readonly string[] Summaries = new[] {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<UserController> _logger;
    private readonly JWTService _jwtService;

    /// <inheritdoc />
    public UserController(ILogger<UserController> logger, JWTService jwtService)
    {
        _logger = logger;
        _jwtService = jwtService;
    }

    [Authorize]
    [HttpGet(Name = "GetWeatherForecast")]
    public string Get()
    {
        return "1";
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
        return ResultModel.Ok(token);
    }
}
