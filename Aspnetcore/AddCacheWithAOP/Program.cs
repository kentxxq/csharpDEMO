using AddCacheWithAOP;
using AspectCore.Extensions.DependencyInjection;
using EasyCaching.Interceptor.AspectCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// 启用响应缓存
builder.Services.AddResponseCaching();

builder.Services.AddTransient<ITestService,TestService>();
builder.Host.UseServiceProviderFactory(new DynamicProxyServiceProviderFactory());
builder.Services.AddEasyCaching(option =>
{
#if (EnableRedis)
        option.UseRedis(builder.Configuration, "redis1")
            .WithSystemTextJson("redis1");
#endif
    option.UseInMemory(builder.Configuration, "memory1");
});
builder.Services.ConfigureAspectCoreInterceptor(_ => { });






var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseResponseCaching();

app.MapControllers();

app.Run();