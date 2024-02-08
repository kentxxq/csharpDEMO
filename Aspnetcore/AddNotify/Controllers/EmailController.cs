using AddNotify.Service.Notify;
using Microsoft.AspNetCore.Mvc;

namespace AddNotify.Controllers;

/// <summary>用户相关</summary>
[ApiExplorerSettings(GroupName = "v1")]
[ApiController]
[Route("[controller]/[action]")]
public class EmailController : ControllerBase
{
    private readonly EmailService _emailService;
    private readonly ILogger<EmailController> _logger;

    /// <inheritdoc />
    public EmailController(ILogger<EmailController> logger, EmailService emailService)
    {
        _logger = logger;
        _emailService = emailService;
    }

    /// <summary>
    ///     测试接口
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<string> A()
    {
        _logger.LogInformation("A");

        await _emailService.SendAsync(
            "kentxxq@dingtalk.com",
            "主题",
            "123\n456\n789",
            @"d:\tmp\新建 XLSX 工作表.xlsx"
        );
        return "A";
    }
}
