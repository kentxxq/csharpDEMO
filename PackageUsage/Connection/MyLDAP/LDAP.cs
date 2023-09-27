using Novell.Directory.Ldap;

namespace MyLDAP;

public static class LDAP
{
    /// <summary>
    /// 测试ldap连接<br />
    /// 等linux上实现了ssl，才能支持636端口 https://github.com/dotnet/runtime/issues/43890
    /// </summary>
    /// <param name="server">服务器地址 ldap.kentxxq.com</param>
    /// <param name="port">端口号 389/636</param>
    /// <param name="userDN">完整的用户DN cn=username,dc=ldap,dc=kentxxq,dc=com</param>
    /// <param name="password">密码 password</param>
    /// <param name="enableSSL">默认false表示port不是ssl端口</param>
    /// <param name="verifySSL">是否验证ssl证书 默认不验证，因为很多都是自签发的。如果需要验证，请在客户端机器导入证书。</param>
    /// <returns>状态正常为true，否则false</returns>
    public static bool VerifyLdapConnection(string server, int port, string userDN, string password,
        bool enableSSL = false, bool verifySSL = false)
    {
        LdapConnection ldapConn;

        if (!verifySSL)
        {
            var op = new LdapConnectionOptions();
            // 证书验证总是返回true
            op.ConfigureRemoteCertificateValidationCallback((_, _, _, _) => true);
            ldapConn = new LdapConnection(op);
        }
        else
        {
            ldapConn = new LdapConnection();
        }

        // 如果启用ssl，这一行必须要有
        if (enableSSL)
        {
            ldapConn.SecureSocketLayer = true;
        }

        try
        {
            ldapConn.Connect(server, port);
            ldapConn.Bind(userDN, password);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}