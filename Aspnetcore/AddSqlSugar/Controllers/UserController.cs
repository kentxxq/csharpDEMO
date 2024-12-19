using System.Text.Encodings.Web;
using System.Text.Json;
using kentxxq.Templates.Aspnetcore.Webapi.Services.UserInfo;
using Microsoft.AspNetCore.Mvc;

namespace AddSqlSugar.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class UserController : ControllerBase
{
    private readonly UserService _userService;

    public UserController(UserService userService)
    {
        _userService = userService;
    }


    /// <summary>
    /// 用户登录
    /// </summary>
    /// <param name="username"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<string> Login(string username, string password)
    {
        var user = await _userService.Login(username, password);
        return JsonSerializer.Serialize(user,new JsonSerializerOptions{
            WriteIndented = true,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        });
    }

}
