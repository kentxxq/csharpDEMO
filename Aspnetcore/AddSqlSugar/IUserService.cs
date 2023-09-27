
using AddSqlSugar.Models;

namespace AddSqlSugar
{
    /// <summary>
    /// 用户接口
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// 登录验证
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        Task<User?> Login(string username, string password);
    }
}
