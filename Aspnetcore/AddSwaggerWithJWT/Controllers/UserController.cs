using AddSwaggerWithJWT.Common;
using AddSwaggerWithJWT.RO;
using AddSwaggerWithJWT.SO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AddSwaggerWithJWT.Controllers;


/// <summary>用户相关</summary>
[ApiExplorerSettings(GroupName = "v1")]
[ApiController]
[Route("[controller]/[action]")]
public class UserController : ControllerBase
{
    private readonly JWTService _jwtService;
    private readonly ILogger<UserController> _logger;

    /// <inheritdoc/>
    public UserController(ILogger<UserController> logger, JWTService jwtService)
    {
        _logger = logger;
        _jwtService = jwtService;
    }

    [AllowAnonymous]
    [HttpPost]
    public ResultModel<LoginSO> Login(LoginRO loginRO)
    {
        if (loginRO.Username != "admin" || loginRO.Password != "123456")
        {
            return ResultModel<LoginSO>.Error("登录失败", new LoginSO());
        }

        var token = _jwtService.GetToken(1, "admin");
        return ResultModel<LoginSO>.Ok(new LoginSO { Token = token });
    }
}
