var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

#region 生命周期

app.Lifetime.ApplicationStarted.Register(() => { Console.WriteLine("ApplicationStarted:启动完成"); });
app.Lifetime.ApplicationStopping.Register(() =>
{
    Console.WriteLine("ApplicationStopping: 10s后关闭");
    // 可以处理term信号 kill -TERM 10060
    var count = 1;
    // shutdown会停止，直到下面的语句执行完成
    while (true)
    {
        if (count > 10)
        {
            break;
        }
        Console.WriteLine(DateTime.Now);
        Thread.Sleep(TimeSpan.FromSeconds(1));
        count++;
    }
});
app.Lifetime.ApplicationStopped.Register(() => { Console.WriteLine("ApplicationStopped:应用已停止"); });

#endregion


app.MapGet("/", () => "Hello World!");

app.Run();