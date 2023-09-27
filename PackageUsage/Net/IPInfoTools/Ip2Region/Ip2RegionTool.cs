using IP2Region.Net.XDB;

namespace IPInfoTools.Ip2Region;

public static class Ip2RegionTool
{
    public static IpServiceModel GetIpInfo(string ip)
    {
        // https://github.com/lionsoul2014/ip2region/blob/master/binding/csharp/README.md
        var search = new Searcher(CachePolicy.Content,"ip2region.xdb");
        var data = search.Search(ip);
        var dataList = data.Split("|");
        var result = new IpServiceModel {
            Status = IpServiceQueryStatus.success,
            Country = dataList[0],
            RegionName = dataList[2],
            Isp = dataList[4],
            City = dataList[3]
        };
        return result;
    }
}