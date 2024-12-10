// hello world是1m
// dotnet publish -r win-x64 -c Release -p:PublishTrimmed=true -p:PublishAot=true -p:StripSymbols=true --self-contained



// winform 使用 Application.StartupPath
// aspnetcore 使用 builder.Environment.ContentRootPath
// 参考
// https://stackoverflow.com/questions/837488/how-can-i-get-the-applications-path-in-a-net-console-application 有回答从net5开始,确定使用AppContext.BaseDirectory
// https://www.hanselman.com/blog/how-do-i-find-which-directory-my-net-core-console-application-was-started-in-or-is-running-from
Console.WriteLine($"当前路径:{AppContext.BaseDirectory}");




// 输出版本号
Console.WriteLine($"dotnet version: {Environment.Version}");
