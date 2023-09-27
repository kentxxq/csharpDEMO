using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Nacos.AspNetCore.V2;
using Nacos.V2;

namespace AddNacos.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class NacosController : ControllerBase
{
    private readonly NacosSettings _appSettings;
    private readonly NacosSettings _mAppSettings;
    private readonly NacosAspNetOptions _nacosAspNetOptions;
    private readonly INacosNamingService _nacosNamingService;
    private readonly NacosSettings _sAppSettings;

    public NacosController(INacosNamingService nacosNamingService,IOptions<NacosAspNetOptions> nacosAspNetOptions,IOptions<NacosSettings> appSetting,IOptionsSnapshot<NacosSettings> sAppSettings,IOptionsMonitor<NacosSettings> mAppSettings)
    {
        _appSettings = appSetting.Value;
        _sAppSettings = sAppSettings.Value;
        _mAppSettings = mAppSettings.CurrentValue;
        _nacosAspNetOptions = nacosAspNetOptions.Value;
        _nacosNamingService = nacosNamingService;
    }
    

    /// <summary>
    /// 获取注册上去的主机连接信息
    /// </summary>
    /// <returns>ip:port</returns>
    [HttpGet]
    public string GetHost()
    {
        var instance = _nacosNamingService.SelectOneHealthyInstance("kentxxq.Templates.Aspnetcore", "dev_demo_group")
            .GetAwaiter()
            .GetResult();
        var host = $"{instance.Ip}:{instance.Port}";
        return host;
    }

    /// <summary>
    /// 获取自己的配置信息
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public List<NacosSettings> GetAppSettings()
    {
        var data = new List<NacosSettings> { _appSettings, _sAppSettings, _mAppSettings };
        return data;
    }

    /// <summary>
    /// 获取注册上去的实例信息
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public NacosAspNetOptions GetNacosAspNetOptions()
    {
        return _nacosAspNetOptions;
    }

}