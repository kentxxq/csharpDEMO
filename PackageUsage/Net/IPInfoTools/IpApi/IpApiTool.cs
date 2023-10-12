using System.Net.Http.Json;
using IPInfoTools.IpApi.Models;

namespace IPInfoTools.IpApi;

public static class IpApiTool
{
    public static async Task<IpServiceModel> GetIpInfo(string ip)
    {
        var httpClient = new HttpClient
        {
            // 默认5秒超时
            Timeout = TimeSpan.FromSeconds(5)
        };
        var data = await httpClient.GetFromJsonAsync<IpApiModel>($"http://ip-api.com/json/{ip}?lang=zh-CN");
        if (data!.Status != "success")
        {
            throw new ApplicationException("查询失败");
        }

        var result = new IpServiceModel
        {
            Status = Enum.Parse<IpServiceQueryStatus>(data.Status), IP = ip, Country = data.Country,
            RegionName = data.RegionName, Isp = data.Isp, City = data.City
        };
        return result;
    }
}
