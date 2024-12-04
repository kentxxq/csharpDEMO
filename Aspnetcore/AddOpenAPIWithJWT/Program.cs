using AddOpenAPIWithJWT;
using AddOpenAPIWithJWT.Common;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
});


builder.Services.AddSingleton<JWTService>();
// builder.Services.AddMySwagger();

builder.Services.AddMyJWT();


var app = builder.Build();


// 是否需要处理401和403
// app.Use(async (context, next) =>
// {
//     await next();
//     switch (context.Response.StatusCode)
//     {
//         case (int)HttpStatusCode.Unauthorized:
//         {
//             context.Response.StatusCode = StatusCodes.Status200OK;
//             context.Response.ContentType = "application/json";
//             var result = ResultModel<string>.Error("token验证失败", "请重新登录或刷新页面");
//             await context.Response.WriteAsJsonAsync(result);
//             break;
//         }
//         case (int)HttpStatusCode.Forbidden:
//         {
//             context.Response.StatusCode = StatusCodes.Status200OK;
//             context.Response.ContentType = "application/json";
//             var result = ResultModel<string>.Error("权限不足", "您没有权限进行此操作");
//             await context.Response.WriteAsJsonAsync(result);
//             break;
//         }
//     }
// });

// 处理异常
// app.UseExceptionHandler(b =>
// {
//     b.Run(async context =>
//     {
//         context.Response.StatusCode = StatusCodes.Status200OK;
//         context.Response.ContentType = "application/json";
//
//         var exception = context.Features.Get<IExceptionHandlerFeature>();
//         if (exception != null)
//         {
//             var result = ResultModel<string>.Error(exception.Error.Message, exception.Error.StackTrace ?? "");
//             await context.Response.WriteAsJsonAsync(result);
//         }
//     });
// });



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // openapi
    app.MapOpenApi();

    // scalar
    // https://github.com/dotnet/aspnetcore/issues/57332 去掉servers内容,scalar就会根据域名自适应base url
    app.MapScalarApiReference(options =>
    {
        options.Servers = [];

        // Basic
        options
            .WithPreferredScheme("Basic") // Security scheme name from the OpenAPI document
            .WithHttpBasicAuthentication(basic =>
            {
                basic.Username = "your-username";
                basic.Password = "your-password";
            });

        options.WithPreferredScheme("Bearer")
            .WithHttpBearerAuthentication(bearer =>
            {
                bearer.Token = "your-bearer-token";
            });
    });

    // swagger
    // controller 注入 ISwaggerProvider 即可在controller获取到注解元数据
    // var swaggerDoc = _swaggerProvider.GetSwagger("v1");
    // var data = swaggerDoc.Paths.ToDictionary(path => path.Key, path => path.Value.Operations.First().Value.Summary);
    // app.UseSwagger(options =>
    // {
    //     options.SerializeAsV2 = true;
    // });
    // app.UseSwaggerUI(u =>
    // {
    //     // 拦截 /swagger/v1/swagger.json 到 SwaggerDoc的v1
    //     u.SwaggerEndpoint($"/openapi/v1.json", "v1");
    //     // 右上角会有2个选项
    //     u.SwaggerEndpoint($"/openapi/v2.json", "v2");
    // });
}

// 加上一个前缀
app.UsePathBase(new PathString($"/{ThisAssembly.Project.AssemblyName}"));

// 认证
app.UseAuthentication();
// 授权
app.UseAuthorization();

// 全局添加需要验证
app.MapControllers();

app.Run();
