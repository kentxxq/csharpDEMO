using Coravel.Queuing.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AddCoravel.Controllers;

[ApiController]
[Route("[controller]")]
public class SchedulerController : ControllerBase
{
    private readonly IQueue _queue;
    private readonly ILogger<SchedulerController> _logger;

    public SchedulerController(IQueue queue, ILogger<SchedulerController> logger)
    {
        _queue = queue;
        _logger = logger;
    }

    [HttpGet]
    public string BackupAllDatabase()
    {
        _queue.QueueInvocable<Hi>();
        _logger.LogInformation("手动触发hi");
        return "触发完成";
    }
}
