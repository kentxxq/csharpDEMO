using System.Text.Json;
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

        // onChange需要被调用一次,才能开启检测. 比如说在controller里,要被请求一次才能监测输出改动. 同样如果在service里,就是要被实例化一次
        // config是现在最新的值, 而name是用来命名区分的,可以省略成(config)=>{}. TopItemSettings在配置的时候,使用了key做区分不同config实例,这里name就是key
        _optionsMonitor.OnChange((config, name) =>
        {
            _logger.LogWarning($"{JsonSerializer.Serialize(config)},{name}");
        });
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
