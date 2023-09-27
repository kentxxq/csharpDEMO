using Coravel.Invocable;

namespace AddCoravel;

// 注意这里如果是带了参数的话，就不能够让ioc来实例化
// builder.Services.AddTransient<TaskWithDIAndParameter>();
public class TaskWithDIAndParameter:IInvocable
{
    private readonly IHostEnvironment _hostEnvironment;
    private readonly string _text;
    
    public TaskWithDIAndParameter(IHostEnvironment hostEnvironment,string text)
    {
        _hostEnvironment = hostEnvironment;
        _text = text;
    }

    public Task Invoke()
    {
        Console.WriteLine($"{_hostEnvironment.ContentRootPath},{_text}");
        return Task.CompletedTask;
    }
}