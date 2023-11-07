var builder = WebApplication.CreateBuilder(args);

// 使用机密
if (!builder.Environment.IsProduction())
{
    builder.Configuration.AddUserSecrets(typeof(Program).Assembly);
}

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

var config = app.Configuration;
// 直接取值
var urls = config["Urls"];
var defaultLogLevel = config["Logging:LogLevel:Default"];
Console.WriteLine($"{urls},{defaultLogLevel}");


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();
