using AddHealthChecks;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

Console.WriteLine(@"就绪检查地址: http://127.0.0.1:5000/healthz/startup");
Console.WriteLine(@"存活检查地址: http://127.0.0.1:5000/healthz");
Console.WriteLine(@"健康检查UI地址: http://127.0.0.1:5000/healthchecks-ui");
    
// 过滤掉无用日志
// .Filter.ByExcluding("RequestPath like '/health%'")
// .MinimumLevel.Override("System.Net.Http.HttpClient.health-checks.LogicalHandler", LogEventLevel.Warning)
// .MinimumLevel.Override("System.Net.Http.HttpClient.health-checks.ClientHandler", LogEventLevel.Warning)

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMyHealthz(builder.Configuration);

builder.Services.AddControllers();

var app = builder.Build();

app.MapHealthChecks("/healthz", new HealthCheckOptions
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.MapHealthChecks("/healthz/startup", new HealthCheckOptions
{
    Predicate = healthCheck => healthCheck.Tags.Contains("startup")
});

app.MapHealthChecksUI();

app.MapControllers();

app.Run();