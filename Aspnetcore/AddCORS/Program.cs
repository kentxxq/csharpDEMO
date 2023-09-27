var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// 跨域配置
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("all",
            policy =>
            {
                policy
                    .SetIsOriginAllowed(_ => true)
                    .AllowCredentials()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
    });
}
    

var app = builder.Build();

app.MapControllers();

app.Run();