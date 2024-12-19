using AddSqlSugar.Models;
using AddSqlSugar.Models.DB;
using Microsoft.Data.Sqlite;
using SqlSugar;

namespace AddSqlSugar;

/// <summary>
/// 初始化静态工具类
/// </summary>
public static class Init
{
    /// <summary>
    /// 初始化数据库
    /// </summary>
    /// <param name="config"></param>
    public static void InitDatabase(IConfiguration config)
    {
        var connectionString = config["Database:ConnectionString"];
        var dbType = (DbType)int.Parse(config["Database:DbType"] ?? "2"); //2是sqlite
        var db = new SqlSugarClient(new ConnectionConfig
        {
            ConnectionString = connectionString,
            DbType = dbType,
            IsAutoCloseConnection = true
        });

        // 即使sqlite文件不存在，CheckConnection也会检查通过
        // 所以单独处理
        if (dbType == DbType.Sqlite)
        {
            var builder = new SqliteConnectionStringBuilder(connectionString);
            if (File.Exists(builder.DataSource))
            {
                Console.WriteLine($"检测到数据库文件{builder.DataSource}");
                SyncTable(db);
            }
            else
            {
                Console.WriteLine($"数据库{builder.DataSource}不存在");
                CreateDatabase(db);
                SyncTable(db);
                InitTableData(db);
            }
        }
        else // 其他类型数据库
        {
            try
            {
                db.Ado.CheckConnection();
                Console.WriteLine("数据库连接成功");
                SyncTable(db);
            }
            catch (Exception)
            {
                Console.WriteLine("数据库连接失败");
                CreateDatabase(db);
                SyncTable(db);
                InitTableData(db);
            }
        }

    }

    private static void CreateDatabase(ISqlSugarClient db)
    {
        var databaseCreated = db.DbMaintenance.CreateDatabase();
        if (!databaseCreated) throw new ApplicationException("数据库创建失败....");
    }

    private static void SyncTable(ISqlSugarClient db)
    {
#pragma warning disable CS8602 // 解引用可能出现空引用。
        var types = typeof(User).Assembly
            .GetTypes()
            .Where(it => it.FullName.StartsWith("AddSqlSugar.Models"))
            .ToArray();
#pragma warning restore CS8602 // 解引用可能出现空引用。
        Console.WriteLine("开始同步表结构");
        db.CodeFirst.SetStringDefaultLength(200).InitTables(types); //根据types创建表
    }

    private static void InitTableData(ISqlSugarClient client)
    {
        var initUser = new User { Username = "ken", Password = "ken", LastLoginTime = DateTime.Now };
        var count = client.Insertable(initUser).ExecuteCommand();
        Console.WriteLine($"初始化表数据-{nameof(User)} {count}条");
    }

}
