using Microsoft.AspNetCore.Diagnostics;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

var app = builder.Build();

// 最外层加上traceId
app.Use(async (context, next) =>
{
    context.Response.Headers.Add("RequestId", context.TraceIdentifier);
    await next();
});

// 处理异常
app.UseExceptionHandler(b =>
{
    b.Run(async context =>
    {
        context.Response.StatusCode = StatusCodes.Status200OK;
        context.Response.ContentType = "text/html";

        var exception = context.Features.Get<IExceptionHandlerFeature>();
        if (exception != null)
        {
            // var result = ResultModel<string>.Error(exception.Error.Message, exception.Error.StackTrace ?? "");
            var result = "报错拉";
            await context.Response.WriteAsJsonAsync(result);
        }
    });
});

// 获取前置nginx代理的数据
// builder.Services.Configure<ForwardedHeadersOptions>(options =>
// {
//     options.ForwardedHeaders = ForwardedHeaders.All;
// });
// 获取nginx代理信息
app.UseForwardedHeaders();

// hsts 告诉浏览器后面所有请求都用https
// app.UseHsts();

// https重定向
app.UseHttpsRedirection();

// blazor等静态文件
// app.UseWebAssemblyDebugging();
// app.UseBlazorFrameworkFiles();
// app.UseStaticFiles();

// 移除掉 /kentxxq.Templates.Aspnetcore 前缀
// 例如请求 kentxxq.com/kentxxq.Templates.Aspnetcore/api/Demo/GetData 就会变成 kentxxq.com/api/Demo/GetData
app.UsePathBase(new PathString("/appName"));
app.UseRouting();

// 跨域、swagger
// if (app.Environment.IsDevelopment())
// {
//     app.UseCors("all");
//     app.UseSwagger();
//     app.UseSwaggerUI(u => { u.SwaggerEndpoint("/swagger/Examples/swagger.json", "Examples"); });
// }

// opentelemetry
// app.UseOpenTelemetryPrometheusScrapingEndpoint();

// 健康检查
// app.MapHealthChecks("/healthz", new HealthCheckOptions
// {
//     Predicate = _ => true,
//     ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
// });
//
// app.MapHealthChecks("/healthz/startup", new HealthCheckOptions
// {
//     Predicate = healthCheck => healthCheck.Tags.Contains("startup")
// });
//
// app.MapHealthChecksUI();

// 简化http记录 builder.Host.UseSerilog();
// app.UseSerilogRequestLogging();

// 开始验证，也就是说上面的所有管道都不需要验证
app.UseAuthentication();
app.UseAuthorization();
// 限速应该在缓存设置前面。否则返回数据的时候，ratelimiter=>cache 429，用户端也会缓存。
app.UseRateLimiter();

// builder.Services.AddResponseCaching();
app.UseResponseCaching();


// razor界面 builder.Services.AddRazorPages();
// app.MapRazorPages();

app.MapControllers();

// websocket
// app.MapHub<ChatHub>("/chatHub");

// 请求失败的页面
// app.MapFallbackToFile("index.html");

app.Run();