using ConfigUsage;

var builder = WebApplication.CreateBuilder(args);
// 默认读取json配置 optional: true,reloadOnChange: tru

var config = builder.Configuration;
// 使用机密
if (!builder.Environment.IsProduction())
{
    config.AddUserSecrets(typeof(Program).Assembly);
}
// 直接获取单个值
var C1 = config.GetValue<string>("C1", "C1-default");
var C2 = config.GetValue<string>("C2:C2-1", "C2-default");
Console.WriteLine($"{C1},{C2}");
// 对象配置
builder.Services.Configure<PositionOptions>(
    builder.Configuration.GetSection(PositionOptions.Position));
// 命名取值.不常用,比如每周六8折,平时9折
builder.Services.Configure<TopItemSettings>(TopItemSettings.Month,
    builder.Configuration.GetSection("TopItem:Month"));
builder.Services.Configure<TopItemSettings>(TopItemSettings.Year,
    builder.Configuration.GetSection("TopItem:Year"));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();
