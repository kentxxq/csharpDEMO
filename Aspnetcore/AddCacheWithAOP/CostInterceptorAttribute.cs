using AspectCore.DependencyInjection;
using AspectCore.DynamicProxy;

namespace AddCacheWithAOP;

/// <summary>
/// 服务耗时标签
/// </summary>
public class CostInterceptorAttribute : AbstractInterceptorAttribute
{
    private DateTime _beginTime;

    [FromServiceContext] public ILogger<CostInterceptorAttribute> _logger { get; set; }

    /// <inheritdoc />
    public async override Task Invoke(AspectContext context, AspectDelegate next)
    {
        try
        {
            _beginTime = DateTime.Now;
            await next(context);
        }
        finally
        {
            var cost = DateTime.Now - _beginTime;
            _logger.LogInformation("计算{ContextImplementation}.{ProxyMethodName}耗时{CostTotalMilliseconds}ms",
                context.Implementation, context.ProxyMethod.Name, cost.TotalMilliseconds);
        }
    }
}