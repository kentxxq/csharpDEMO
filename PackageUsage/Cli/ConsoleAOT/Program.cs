// hello world是1m
// dotnet publish -r win-x64 -c Release -p:PublishTrimmed=true -p:PublishAot=true -p:StripSymbols=true --self-contained

Console.WriteLine($"当前路径:{AppContext.BaseDirectory}");
// aspnetcore 使用 builder.Environment.ContentRootPath
