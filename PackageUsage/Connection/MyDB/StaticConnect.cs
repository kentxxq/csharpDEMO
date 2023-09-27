using SqlSugar;

namespace MyDB;

public static class StaticConnect
{
    /// <summary>
    /// mysql同步连接测试
    /// </summary>
    /// <returns>bool值确定是否连接成功</returns>
    public static bool TryConnectDB(string connectionString,DbType dbType)
    {
        var db= new SqlSugarClient(new ConnectionConfig{
                ConnectionString = connectionString, 
                DbType = dbType,
                IsAutoCloseConnection = true},
            db=>
            {
                db.Aop.OnLogExecuting = (sql, pars) =>
                {
                    Console.WriteLine(sql); //输出sql,查看执行sql 性能无影响
                };
            });
        return db.Ado.IsValidConnection();
    }
}