using AddSwaggerWithJWT;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();


builder.Services.AddSingleton<JWTService>();
builder.Services.AddMySwagger()
    .AddMyJWT();


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
    app.UseSwagger();
    app.UseSwaggerUI(u => { u.SwaggerEndpoint("/swagger/Examples/swagger.json", "Examples"); });
}

app.MapControllers();

app.Run();