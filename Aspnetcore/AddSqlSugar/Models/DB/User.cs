﻿using SqlSugar;

namespace AddSqlSugar.Models.DB;

[SugarTable(tableName: nameof(User), tableDescription: "用户")]
public class User
{
    [SugarColumn(IsIdentity = true, IsPrimaryKey = true)]
    public int Id { get; set; }

    [SugarColumn] public string Username { get; set; } = null!;

    [SugarColumn] public string Password { get; set; } = null!;

    [SugarColumn(IsNullable = true)] public DateTime? LastLoginTime { get; set; }
}
