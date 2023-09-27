// See https://aka.ms/new-console-template for more information

using System.Net;
using System.Net.NetworkInformation;
using MyPing;

var reply = StaticPing.Ping("www.baidu.com");
Console.WriteLine(reply.Status==IPStatus.Success);

var ip = IPAddress.Parse("223.5.5.5");
var result = await StaticPing.PingIpAsync(ip);
Console.WriteLine(result);