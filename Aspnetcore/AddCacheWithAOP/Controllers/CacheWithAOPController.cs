using Microsoft.AspNetCore.Mvc;

namespace AddCacheWithAOP.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class CacheWithAOPController : ControllerBase
{
    private readonly ITestService _testService;

    public CacheWithAOPController(ITestService testService)
    {
        _testService = testService;
    }

    [HttpGet]
    public string GetData()
    {
        var result = _testService.GetData();
        return result;
    }
    
    [HttpGet]
    public async Task<DateTime> GetDateTime()
    {
        var result =await _testService.GetLocalCacheData();
        return result;
    }

    /// <summary>
    /// 客户端缓存10s，不会发送新的请求过来
    /// location.client代表只能在浏览器存储，不能由cdn，nginx等中间层缓存. any代表中间层也可以缓存。none可以理解为代表no-cache，仅中间件缓存。no-store单独设置bool值，直接不缓存
    /// https://learn.microsoft.com/zh-cn/aspnet/core/performance/caching/response?view=aspnetcore-6.0#http-based-response-caching
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ResponseCache(Duration = 10, Location = ResponseCacheLocation.Client)]
    public async Task<string> TestClientCache()
    {
        await Task.Delay(2000);
        return "1";
    }
}