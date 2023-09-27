using AddNacos;
using Nacos.AspNetCore.V2;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



// nacos 服务注册与发现
builder.Services.AddNacosAspNet(builder.Configuration, "NacosConfig");
// nacos 配置中心
builder.Configuration.AddNacosV2Configuration(builder.Configuration.GetSection("NacosConfig"));
// nacos 读取对应配置到对象
builder.Services.Configure<NacosSettings>(builder.Configuration.GetSection("NacosSettings"));




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();