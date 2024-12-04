using System.ComponentModel;

namespace AddOpenAPIWithJWT.RO;

public class LoginRO
{
    [Description("用户名")]
    public string Username { get; set; }
    [Description("密码")]
    public string Password { get; set; }
}
