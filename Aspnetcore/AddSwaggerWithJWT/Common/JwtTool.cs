﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using IdentityModel;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.IdentityModel.Tokens;

namespace AddSwaggerWithJWT.Common;

public static class JwtTool
{
    /// <summary>
    /// 快速创建可用的token
    /// </summary>
    /// <param name="uid">用户id</param>
    /// <param name="username">用户名</param>
    /// <param name="expires">什么时候过期</param>
    /// <param name="issuer">签发者，默认ken</param>
    /// <param name="schemaName">验证方案名称，默认Bearer</param>
    /// <param name="secret">密码</param>
    /// <returns></returns>
    public static string CreateTokenString(int uid, string username, string secret, DateTimeOffset expires,
        string issuer = "ken", string schemaName = "Bearer")
    {
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Claims = new Dictionary<string, object>
            {
                { JwtClaimTypes.Subject, schemaName },
                { JwtClaimTypes.Id, uid },
                { JwtClaimTypes.NickName, username }
            },
            // 签证机构的名称
            Issuer = issuer,
            // 受众。签证机构把认证给了ken
            Audience = issuer,
            // 签发时间
            IssuedAt = DateTimeOffset.Now.UtcDateTime,
            // 在这之前不可用.作用是1点签发token，允许2点开始生效，生效1小时到3点
            NotBefore = DateTimeOffset.Now.UtcDateTime,
            // 什么时候过期
            Expires = expires.UtcDateTime,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Convert.FromBase64String(secret)),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        var token = tokenHandler.CreateToken(tokenDescriptor);

        var stringToken = tokenHandler.WriteToken(token);

        return stringToken;
    }

    /// <summary>
    /// 字符串转jwt对象
    /// </summary>
    /// <param name="jwtString"></param>
    /// <returns></returns>
    /// <exception cref="ApplicationException"></exception>
    public static JwtSecurityToken ParseJWTStringToJwtSecurityToken(string jwtString)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadToken(jwtString) as JwtSecurityToken;
        if (jwt is null) throw new ApplicationException("token解析失败！");
        return jwt;
    }

    /// <summary>
    /// 字符串转AuthenticationState
    /// </summary>
    /// <param name="jwtString"></param>
    /// <returns></returns>
    public static AuthenticationState ParseJWTStringToAuthenticationState(string jwtString)
    {
        var jwt = ParseJWTStringToJwtSecurityToken(jwtString);
        return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(jwt.Claims, "jwt")));
    }

    /// <summary>
    /// 获取token的剩余的时间（秒数）
    /// </summary>
    /// <param name="jwtString"></param>
    /// <returns></returns>
    public static int GetExpSecondsFromToken(string jwtString)
    {
        var jwt = ParseJWTStringToJwtSecurityToken(jwtString);
        var tokenExp = jwt.Claims.First(c => c.Type == "exp").Value;
        var exp = long.Parse(tokenExp);
        var expDateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(exp);
        return (int)(expDateTimeOffset - DateTimeOffset.Now).TotalSeconds;
    }

    /// <summary>
    /// 获取AuthenticationState的剩余的时间（秒数）
    /// </summary>
    /// <param name="authenticationState"></param>
    /// <returns></returns>
    public static int GetExpSecondsFromAuthenticationState(AuthenticationState authenticationState)
    {
        var expClaim = authenticationState.User.Claims.FirstOrDefault(c => c.Type == "exp");
        if (expClaim is null) return -1;
        var exp = long.Parse(expClaim.Value);
        var expDateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(exp);
        return (int)(expDateTimeOffset - DateTimeOffset.Now).TotalSeconds;
    }
}
