using System.Text.Encodings.Web;
using System.Text.Json;
using Cli.Utils.Ip;
using IPInfoTools.Ip2Region;
using IPInfoTools.IpApi;

var result = await IpApiTool.GetIpInfo("175.9.143.159");
Console.WriteLine(JsonSerializer.Serialize(result,new JsonSerializerOptions {
    WriteIndented = true,
    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping // unicode转换成中文
}));

var result2 = Ip2RegionTool.GetIpInfo("175.9.143.159");
Console.WriteLine(JsonSerializer.Serialize(result2,new JsonSerializerOptions {
    WriteIndented = true,
    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping // unicode转换成中文
}));

var result3 = await IpService.GetMyIP();
Console.WriteLine($"我的ip: {result3}");

var result4 = await IpService.ImInChina();
Console.WriteLine($"在中国？{result4}");

