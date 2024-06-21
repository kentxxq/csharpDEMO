using AddSerilog;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .AddDefaultLogConfig()
    .CreateBootstrapLogger();

Log.Information("日志初始化完成,正在启动服务...");

try
{
    builder.Host.UseSerilog((serviceProvider, loggerConfiguration) =>
    {
        loggerConfiguration.AddCustomLogConfig(builder.Configuration);
    });

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
