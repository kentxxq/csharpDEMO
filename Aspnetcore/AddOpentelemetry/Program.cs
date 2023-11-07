using System.Diagnostics;
using AddOpenTelemetry;

Console.WriteLine(@"metrics监控地址: http://127.0.0.1:5000/metrics");

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddUserSecrets(typeof(Program).Assembly);

// 统一注入
builder.AddMyOpenTelemetry();

builder.Services.AddControllers();
builder.Services.AddSingleton<IDemoService, DemoService>();

var app = builder.Build();

// header添加TraceId
app.Use(async (context, next) =>
{
    context.Response.Headers.Add("TraceId", Activity.Current?.TraceId.ToString());
    await next();
});

app.UseOpenTelemetryPrometheusScrapingEndpoint();

app.MapControllers();

app.Run();
