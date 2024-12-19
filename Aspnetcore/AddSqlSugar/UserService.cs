
using AddSqlSugar;
using AddSqlSugar.Models;
using AddSqlSugar.Models.DB;
using SqlSugar;

namespace kentxxq.Templates.Aspnetcore.Webapi.Services.UserInfo
{
    /// <inheritdoc />
    public class UserService
    {
        private readonly ISqlSugarClient _sqlSugarClient;

        /// <summary>
        /// 依赖注入
        /// </summary>
        public UserService(ISqlSugarClient sqlSugarClient)
        {
            _sqlSugarClient = sqlSugarClient;
        }

        /// <inheritdoc />
        public async Task<User?> Login(string username, string password)
        {
            var user = await _sqlSugarClient.Queryable<User>()
                .Where(u => u.Username == username && u.Password == password)
                .FirstAsync();
            return user;
        }
    }
}
