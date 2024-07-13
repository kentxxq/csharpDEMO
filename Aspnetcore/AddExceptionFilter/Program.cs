using System.Net.Mime;
using AddExceptionFilter;
using Microsoft.AspNetCore.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

// 使用filter
builder.Services.AddControllers(options => { options.Filters.Add<PusherExceptionFilter>();});

var app = builder.Build();

// 拦截pusherException之外的异常, pusherException用filter处理
app.UseExceptionHandler(exceptionHandlerApp =>
{
    exceptionHandlerApp.Run(async context =>
    {
        context.Response.StatusCode = StatusCodes.Status200OK;
        context.Response.ContentType = MediaTypeNames.Application.Json;
        var exceptionHandlerPathFeature =
            context.Features.Get<IExceptionHandlerPathFeature>();
        ResultModel<string> result;
        if (app.Environment.IsDevelopment())
        {
            // 包含代码结构，属于敏感信息
            result = ResultModel.Error(exceptionHandlerPathFeature?.Error.Message ?? string.Empty,
                exceptionHandlerPathFeature?.Error.StackTrace ?? string.Empty);
        }
        else
        {
            result = ResultModel.Error(exceptionHandlerPathFeature?.Error.Message ?? string.Empty,
                exceptionHandlerPathFeature?.Error.Source ?? string.Empty);
        }

        await context.Response.WriteAsJsonAsync(result);
    });
});

// 处理403,401等状态码
app.UseStatusCodePages(async statusCodeContext =>
{
    statusCodeContext.HttpContext.Response.ContentType = MediaTypeNames.Application.Json;
    var result = ResultModel.Error(statusCodeContext.HttpContext.Response.StatusCode.ToString(),
        statusCodeContext.HttpContext.Response.StatusCode);
    await statusCodeContext.HttpContext.Response.WriteAsJsonAsync(result);
});


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
