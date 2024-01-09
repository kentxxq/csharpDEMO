using Serilog;
using Serilog.Logfmt;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    // .MinimumLevel.Override("xx",Serilog.Events.LogEventLevel.Warning)
    .Enrich.When(logEvent=>!logEvent.Properties.ContainsKey("SourceContext"),enrichmentConfig=>enrichmentConfig.WithProperty("SourceContext","SourceContext"))
    .Enrich.When(logEvent=>!logEvent.Properties.ContainsKey("ThreadName"),enrichmentConfig=>enrichmentConfig.WithProperty("ThreadName","ThreadName"))
    .ReadFrom.Configuration(builder.Configuration)
    // logfmt格式我只用于写入到控制台，文件都建议采用json格式。
    // logfmt对比竖线分隔符，优势如下：
    // - 如果需要输出所有的字段，而不确定有哪些字段的时候。例如sourceContext
    // - 字段很多，记不住顺序。而key-value的展示起来很清晰

    // 要使用下面的配置，需要注释掉json配置中的console配置
    // .WriteTo.Console(formatter:new LogfmtFormatter(opt =>
    // {
    //     // 双引号用单引号代替
    //     opt.OnDoubleQuotes(q => q.ConvertToSingle());
    //     // 打印所有的额外sourceContext
    //     opt.IncludeAllProperties();
    //     // 保持原有的大小写模式。默认会变成下划线分割，例如XiaoLi变成xiao_li
    //     opt.PreserveCase();
    // }))
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
