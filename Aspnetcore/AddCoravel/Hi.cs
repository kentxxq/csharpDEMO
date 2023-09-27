using Coravel.Invocable;

namespace AddCoravel;

// 实现IInvocable接口
public class Hi:IInvocable
{
    public Task Invoke()
    {
        Console.WriteLine("Hi");
        return Task.CompletedTask;
    }
}

