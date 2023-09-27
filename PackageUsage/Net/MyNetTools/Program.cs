// See https://aka.ms/new-console-template for more information

using MyNetTools;


Console.WriteLine($"本机ip:{StaticNetTool.GetLocalIP()}");
Console.WriteLine($"本机mac:{StaticNetTool.GetLocalMacString()}");
Console.WriteLine($"网络掩码:{StaticNetTool.GetNetMask()}");
Console.WriteLine($"本机网关:{StaticNetTool.GetLocalGateway()}");

Console.WriteLine($"网关mac:{StaticNetTool.GetPhysicalAddressByIp(StaticNetTool.GetLocalGateway()!)}");
var device = StaticNetTool.GetNetDevice();
Console.WriteLine($"当前的网卡设备是:{device?.Name},{device?.Description}");


var subnetIp = StaticNetTool.GetAllSubnetIp().ToList();
Console.WriteLine($"同网段有{subnetIp.Count}个ip");
Console.WriteLine("通过icmp检测，存活的ip如下");
var aliveIp = StaticNetTool.GetAliveIpListByICMP();
foreach (var ip in aliveIp)
{
    Console.WriteLine($"{ip},{StaticNetTool.GetPhysicalAddressByIp(ip)}");
}