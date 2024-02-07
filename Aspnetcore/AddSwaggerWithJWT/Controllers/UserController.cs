using AddSwaggerWithJWT.Common;
using AddSwaggerWithJWT.Response;
using AddSwaggerWithJWT.RO;
using AddSwaggerWithJWT.SO;
using Microsoft.AspNetCore.Mvc;

namespace AddSwaggerWithJWT.Controllers;

/// <summary>用户相关</summary>
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
