using AddEventListener;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();



builder.Services.AddMyEventListener();



var app = builder.Build();

app.MapControllers();

app.Run();