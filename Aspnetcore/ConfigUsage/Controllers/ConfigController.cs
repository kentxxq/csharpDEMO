using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ConfigUsage.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class ConfigController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly IOptions<PositionOptions> _options;
    private readonly IOptionsMonitor<TopItemSettings> _optionsMonitor;
    private readonly ILogger<ConfigController> _logger;

    // IOptions 不能修改,随便用
    // IOptionsSnapshot 性能不好,特殊场景使用. 每次请求都会重新计算, 只能用在scope中. 适用于:你的配置是动态的.并且你每次执行代码,都是读取当时的值.
    // IOptionsMonitor 更新变化.有通知
    public ConfigController(ILogger<ConfigController> logger, IConfiguration configuration,IOptions<PositionOptions> options, IOptionsMonitor<TopItemSettings> optionsMonitor)
    {
        _logger = logger;
        _configuration = configuration;
        _options = options;
        _optionsMonitor = optionsMonitor;
    }

    [HttpGet]
    public string Get()
    {
        // 注入取值
        _logger.LogWarning($"{_options.Value.Title},{_options.Value.Name}");

        // 命名取值
        _logger.LogWarning($"{_optionsMonitor.Get(TopItemSettings.Year).Name},{_optionsMonitor}");

        return "1";
    }
}
