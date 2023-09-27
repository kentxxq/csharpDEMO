using EasyCaching.Core.Interceptor;

namespace AddCacheWithAOP;

public interface ITestService
{
    [CostInterceptor]
    string GetData();
    
    /// <summary>
    /// 获取本地缓存数据
    /// </summary>
    /// <returns></returns>
    [EasyCachingAble(CacheProviderName = "memory1", Expiration = 10)]
    Task<DateTime> GetLocalCacheData();
}

public class TestService:ITestService
{
    public string GetData()
    {
        return "!23";
    }

    public Task<DateTime> GetLocalCacheData()
    {
        return Task.FromResult(DateTime.Now);
    }
}