### schedule配置

#### 1. builder配置服务

```c#
builder.Services.AddScheduler();
```

#### 简单使用

```c#
var provider = app.Services;
provider.UseScheduler(scheduler =>
{
    scheduler.Schedule(() => { Console.WriteLine(DateTime.Now); })
        .EverySeconds(5);
});
```

#### 标准使用

```c#
// 实现IInvocable接口
public class Hi:IInvocable
{
    public Task Invoke()
    {
        Console.WriteLine("Hi");
        return Task.CompletedTask;
    }
}

// 注入它！！！
builder.Services.AddTransient<Hi>();

// 配置调用它
scheduler.Schedule<Hi>().EverySeconds(5);
```

#### 全面使用

```c#
// 注意这里如果是带了参数的话，就不能够让ioc来实例化
// builder.Services.AddTransient<TaskWithDIAndParameter>();
public class TaskWithDIAndParameter:IInvocable
{
    private readonly IHostEnvironment _hostEnvironment;
    private readonly string _text;
    
    public TaskWithDIAndParameter(IHostEnvironment hostEnvironment,string text)
    {
        _hostEnvironment = hostEnvironment;
        _text = text;
    }

    public Task Invoke()
    {
        Console.WriteLine($"{_hostEnvironment.ContentRootPath},{_text}");
        return Task.CompletedTask;
    }
}

//分隔符// 
app.Services.UseScheduler(scheduler =>
    {
        // scheduler.Schedule(() => { Console.WriteLine(DateTime.Now); })
        //     .EverySeconds(5);
    
        scheduler.Schedule<Hi>()
            .EverySecond();
    
        scheduler.OnWorker(nameof(TaskWithDIAndParameter)); // 下面的任务在独立线程中执行
        scheduler.ScheduleWithParams<TaskWithDIAndParameter>("TaskWithDIAndParameter")
            .EverySecond()
            .RunOnceAtStart() // 启动后运行一次，比如设置了cron的时候很有用
            .When(() => Task.FromResult(true)) // 触发条件
            .PreventOverlapping(nameof(TaskWithDIAndParameter)); // 确保只有一个在运行,上一个还在跑，现在的就不跑了
    })
    .OnError(exception =>
    {
        Console.WriteLine(exception.Message);
    })// 全局错误处理
    .LogScheduledTaskProgress(app.Services.GetService<ILogger<IScheduler>>());// 记录任务执行(日志级别是debug)
```

#### 说明

这个库还有queue和broadcast。但是我觉得应该通过kafka/mq来投递消息。所以就不写无关的内容了