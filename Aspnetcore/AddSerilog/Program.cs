using AddSerilog;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var instanceId = Guid.NewGuid().ToString();

// 不建议使用CreateBootstrapLogger. 有时候会导致没有log,也和senparc.weixin的冲突...
// 问题: https://github.com/serilog/serilog-sinks-file/issues/220  https://github.com/JeffreySu/WeiXinMPSDK/issues/3047
// Log.Logger = new LoggerConfiguration()
//     .AddDefaultLogConfig()
//     .CreateBootstrapLogger();
// Log.Information("日志初始化完成,正在启动服务...");

try
{
    var enableOpentelemetry = builder.Configuration.GetValue("EnableOpenTelemetry", false);

    // AddMyOpenTelemetry必须一起使用
    // if (enableOpentelemetry)
    // {
    //     // 必须在AddMyOpenTelemetry之前,不能和下面的放在一起
    //     builder.AddMyOpenTelemetry(instanceId);
    // }

    builder.Host.UseSerilog((serviceProvider, loggerConfiguration) =>
    {
        loggerConfiguration
            .AddCustomLogConfig(builder.Configuration);
        // 必须和AddMyOpenTelemetry配合使用
        // if (enableOpentelemetry)
        // {
        //     loggerConfiguration.AddMyOpenTelemetry(builder.Configuration,instanceId);
        // }
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
