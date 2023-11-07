using OpenTelemetry;
using OpenTelemetry.Exporter;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace AddOpenTelemetry;

/// <summary>OpenTelemetry-拓展方法</summary>
public static class MyOpenTelemetryExtension
{
    private const string AppName = ThisAssembly.Project.AssemblyName;
    private const string AppVersion = ThisAssembly.Info.Version;
    private static string _ocEndpoint = string.Empty;

    /// <summary>添加OpenTelemetry</summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static WebApplicationBuilder AddMyOpenTelemetry(this WebApplicationBuilder builder)
    {
        _ocEndpoint = builder.Configuration["OC_Endpoint"] ??
                      throw new InvalidOperationException("必须配置open telemetry的collector地址");

        var openTelemetryBuilder = builder.Services.AddOpenTelemetry()
            .ConfigureResource(resourceBuilder => resourceBuilder.AddService(AppName, serviceVersion: AppVersion));

        AddLogging(builder);
        AddMetrics(openTelemetryBuilder);
        AddTrace(openTelemetryBuilder);

        return builder;
    }

    /// <summary>添加logging</summary>
    /// <param name="builder"></param>
    private static void AddLogging(WebApplicationBuilder builder)
    {
        builder.Logging.AddOpenTelemetry(lo =>
        {
            // 包含attributes字段，里面有RequestUrl，ConnectionId等信息
            lo.IncludeScopes = true;
            // var resourceBuilder = ResourceBuilder.CreateDefault().AddService(AppName, serviceVersion: AppVersion);
            // lo.SetResourceBuilder(resourceBuilder);
            lo.AddOtlpExporter(options =>
            {
                options.Endpoint = new Uri(_ocEndpoint);
                options.Protocol = OtlpExportProtocol.Grpc;
            });
        });
    }

    /// <summary>添加 metrics 指标数据</summary>
    /// <param name="openTelemetryBuilder"></param>
    private static void AddMetrics(OpenTelemetryBuilder openTelemetryBuilder)
    {
        openTelemetryBuilder
            .WithMetrics(builder => builder
                // .AddConsoleExporter((options, readerOptions) =>
                // {
                //     readerOptions.PeriodicExportingMetricReaderOptions.ExportIntervalMilliseconds = 1000;
                // })
                // debug用
                .AddPrometheusExporter()
                // .AddOtlpExporter(o =>
                // {
                //     o.Endpoint = new Uri("http://poc.mashibing.com:4317");
                //     o.Protocol = OtlpExportProtocol.Grpc;
                // })
                .AddOtlpExporter((options, readerOptions) =>
                {
                    options.Endpoint = new Uri(_ocEndpoint);
                    options.Protocol = OtlpExportProtocol.Grpc;

                    readerOptions.PeriodicExportingMetricReaderOptions.ExportIntervalMilliseconds = 1000;
                })
                .AddMeter(AppName)
                .AddHttpClientInstrumentation()
                .AddAspNetCoreInstrumentation()
                .AddRuntimeInstrumentation()
                .AddEventCountersInstrumentation(o =>
                {
                    o.RefreshIntervalSecs = 1;
                    // https://learn.microsoft.com/en-us/dotnet/core/diagnostics/available-counters
                    // o.AddEventSources("System.Runtime"); 有RuntimeInstrumentation和AspNetCoreInstrumentation就够了
                    o.AddEventSources("Microsoft.AspNetCore.Hosting");
                    // o.AddEventSources("Microsoft.AspNetCore.Http.Connections"); 没看到输出
                    o.AddEventSources("Microsoft-AspNetCore-Server-Kestrel");
                    o.AddEventSources("System.Net.Http");
                    o.AddEventSources("System.Net.NameResolution");
                    // o.AddEventSources("System.Net.Security"); 主要是ssl信息，挂在代理后面不需要这个
                    o.AddEventSources("System.Net.Sockets");
                })
            );
    }

    /// <summary>添加 trace 追踪数据</summary>
    /// <param name="openTelemetryBuilder"></param>
    private static void AddTrace(OpenTelemetryBuilder openTelemetryBuilder)
    {
        openTelemetryBuilder.WithTracing(builder =>
            builder.AddAspNetCoreInstrumentation(o => { o.Filter = r => r.Request.Path != "/healthz"; })
                .AddGrpcClientInstrumentation(o =>
                {
                    // grpc基于http,如果为true,那么http就可以检测到grpc的traceid,产生一些activity.所以默认false关掉它.
                    o.SuppressDownstreamInstrumentation = false;
                })
                .AddHttpClientInstrumentation(o =>
                {
                    o.FilterHttpRequestMessage = r => r.RequestUri?.PathAndQuery != "/healthz";
                })
                .AddQuartzInstrumentation()
                // 默认 ParentBased(root=AlwaysOn) 全部采集
                // .SetSampler(new TraceIdRatioBasedSampler(0.1))  // 采集1/10的指标
                // debug用
                // .AddConsoleExporter()
                .AddOtlpExporter(o =>
                {
                    o.Endpoint = new Uri(_ocEndpoint);
                    o.Protocol = OtlpExportProtocol.Grpc;
                }));

        // docker run -d --name zipkin -p 9411:9411 openzipkin/zipkin
        // .AddZipkinExporter(o =>
        // {
        //     o.Endpoint = new Uri("http://8.142.70.33:9411/api/v2/spans");
        // });

        // docker run -d --name jaeger \
        // -e COLLECTOR_ZIPKIN_HOST_PORT=:9411 \
        // -e COLLECTOR_OTLP_ENABLED=true \
        // -e JAEGER_SAMPLER_TYPE=const \
        // -e JAEGER_SAMPLER_PARAM=1 \
        // -e JAEGER_REPORTER_LOG_SPANS=true \
        // -p 6831:6831/udp \
        // -p 6832:6832/udp \
        // -p 5778:5778 \
        // -p 16686:16686 \
        // -p 4317:4317 \
        // -p 4318:4318 \
        // -p 14250:14250 \
        // -p 14268:14268 \
        // -p 14269:14269 \
        // -p 9411:9411 \
        // jaegertracing/all-in-one:1.39
        // Jaeger支持了OTLP,不再需要引入exporter依赖 https://www.jaegertracing.io/docs/1.48/apis/#opentelemetry-protocol-stable
    }
}
