using System.Diagnostics;
using AddOpenTelemetry;
using OpenTelemetry.Exporter;
using OpenTelemetry.Logs;

Console.WriteLine(@"metrics监控地址: http://127.0.0.1:5000/metrics");

var builder = WebApplication.CreateBuilder(args);

// 注入指标,追踪
builder.Services.AddMyOpenTelemetry();

// 日志
builder.Logging.AddOpenTelemetry(lo =>
{
    lo.AddOtlpExporter((options) =>
    {
        options.Endpoint = new Uri("http://192.168.31.210:4317");
        options.Protocol = OtlpExportProtocol.Grpc;
    });
});

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
