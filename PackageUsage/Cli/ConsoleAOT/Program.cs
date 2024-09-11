using Microsoft.Extensions.Hosting;

// aot打包有4m,hello world是1m
// dotnet publish -r win-x64 -c Release -p:PublishTrimmed=true -p:PublishAot=true -p:StripSymbols=true --self-contained

// Host.CreateApplicationBuilder() 替代 Host.CreateDefaultBuilder() 因为 dotnet 的代码基调是从 callback 转向线性代码
// 相关讨论参考
// https://stackoverflow.com/questions/75229793/host-createdefaultbuilder-vs-host-createapplicationbuilder-in-net-platform-exte
// https://github.com/dotnet/runtime/issues/61634
var builder = Host.CreateApplicationBuilder();

// builder.configureService 变成 builder.Services 就是 callback 转向线性代码
Console.WriteLine($"services: {builder.Services.Count}");

var host = builder.Build();

await host.RunAsync();
