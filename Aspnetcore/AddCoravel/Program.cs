using AddCoravel;
using Coravel;
using Coravel.Scheduling.Schedule.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScheduler();


var app = builder.Build();

// 注入它！！！
builder.Services.AddTransient<Hi>();

var provider = app.Services;
provider.UseScheduler(scheduler =>
{
    scheduler.Schedule(() => { Console.WriteLine(DateTime.Now); })
        .EverySeconds(5);
    scheduler.Schedule<Hi>().EverySeconds(5);
    
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
    .LogScheduledTaskProgress(app.Services.GetService<ILogger<IScheduler>>());// 记录任务执行(日志级别是debug);


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();