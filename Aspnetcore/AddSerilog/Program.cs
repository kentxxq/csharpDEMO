using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.ForContext("Project", ThisAssembly.Project.AssemblyName);
Log.Logger = new LoggerConfiguration()
    // .MinimumLevel.Override("xx",Serilog.Events.LogEventLevel.Warning)
    .Enrich.WithProperty("AppName",ThisAssembly.Project.AssemblyName)
    .Enrich.When(logEvent=>!logEvent.Properties.ContainsKey("SourceContext"),enrichmentConfig=>enrichmentConfig.WithProperty("SourceContext","SourceContext"))
    .Enrich.When(logEvent=>!logEvent.Properties.ContainsKey("ThreadName"),enrichmentConfig=>enrichmentConfig.WithProperty("ThreadName","ThreadName"))
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

Log.Information("启动中...");

try
{
    builder.Host.UseSerilog();

    builder.Services.AddControllers();

    var app = builder.Build();

    // 简化http输出
    app.UseSerilogRequestLogging();

    app.MapControllers();

    app.Run();
}

catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
