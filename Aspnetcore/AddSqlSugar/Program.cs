using AddSqlSugar;
using kentxxq.Templates.Aspnetcore.Webapi.Services.UserInfo;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



Init.InitDatabase(builder.Configuration);
// 数据库
builder.AddSqlsugarSetup();
builder.Services.AddTransient<IUserService, UserService>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
