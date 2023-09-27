// See https://aka.ms/new-console-template for more information

using System.Text;
using MyEncrypt;
using DemoData;




Console.WriteLine($"数据为:{StaticData.StringData},密码为:{StaticData.Password}");

Console.WriteLine($"使用aes加密中...");
var dataBytes = Encoding.UTF8.GetBytes(StaticData.StringData);
var encryptBytes = KAes.EncryptBytes(dataBytes,"123456");
Console.WriteLine(BitConverter.ToString(encryptBytes));


Console.WriteLine("-------------------------------");
Console.WriteLine($"开始计算sha256值");
Console.WriteLine(StaticData.StringData.Sha256());

