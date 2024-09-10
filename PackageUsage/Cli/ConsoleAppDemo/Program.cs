using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder();

// 不再使用 new ServiceCollection() , 使用 builder.Services 替代
Console.WriteLine($"services: {builder.Services.Count}");

var host = builder.Build();

await host.RunAsync();
