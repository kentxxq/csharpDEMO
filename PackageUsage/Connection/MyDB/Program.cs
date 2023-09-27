// See https://aka.ms/new-console-template for more information

using MyDB;
using SqlSugar;

var result =
    StaticConnect.TryConnectDB(
        @"Server=数据库地址;Database=testdb;Uid=root;Pwd=123456;MinimumPoolSize=10;maximumpoolsize=50;", DbType.MySql);
Console.WriteLine(result);
