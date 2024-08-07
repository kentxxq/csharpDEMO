using System.Diagnostics;
using AddOpenTelemetry;

Console.WriteLine(@"metrics监控地址: http://127.0.0.1:5000/metrics");

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddUserSecrets(typeof(Program).Assembly);

// 统一注入
var instanceId = Guid.NewGuid().ToString();
builder.AddMyOpenTelemetry(instanceId);

builder.Services.AddControllers();
builder.Services.AddSingleton<IDemoService, DemoService>();

var app = builder.Build();

// header添加TraceId
app.Use(async (context, next) =>
{
    context.Response.Headers.Append("TraceId", Activity.Current?.TraceId.ToString());
    await next();
});

app.UseOpenTelemetryPrometheusScrapingEndpoint();

app.MapControllers();

app.Run();
