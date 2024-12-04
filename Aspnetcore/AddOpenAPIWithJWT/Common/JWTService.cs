using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace AddOpenAPIWithJWT.Common;

/// <summary>
/// JWT工具
/// </summary>
public class JWTService
{
    private readonly IConfiguration _configuration;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="configuration"></param>
    public JWTService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    /// <summary>
    /// 获取token
    /// </summary>
    /// <param name="uid">用户id</param>
    /// <param name="username">用户名</param>
    /// <param name="schemaName">验证方案名称，默认Bearer</param>
    /// <returns></returns>
    public string GetToken(int uid, string username, string schemaName = JwtBearerDefaults.AuthenticationScheme)
    {
        var issuer = _configuration.GetValue<string>($"Authentication:Schemes:{schemaName}:ValidIssuer") ?? string.Empty;
        var signingKey = _configuration.GetSection($"Authentication:Schemes:{schemaName}:SigningKeys")
            .GetChildren()
            .SingleOrDefault(key => key["Issuer"] == issuer);
        var secret = string.Empty;
        if (signingKey?["Value"] is { } keyValue) secret = keyValue;

        return JwtTool.CreateTokenString(uid, username, secret, DateTimeOffset.Now.AddHours(3),issuer);
    }
}
