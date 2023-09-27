using BenchmarkDotNet.Attributes;

namespace MyBenchmark;

public class TaskBenchmark
{
    private const string TestServerUrl = "http://39.103.193.37/200";
    private static readonly HttpClient _client = new();

    [Benchmark]
    public void TestThreadPool()
    {
        ThreadPool.SetMaxThreads(500, 500);
        for (var i = 0; i < 10000; i++)
        {
            ThreadPool.QueueUserWorkItem(SendRequest);
        }

        while (ThreadPool.CompletedWorkItemCount == 10000)
        {
            break;
        }
    }

    private void SendRequest(object stateInfo)
    {
        _client.GetStringAsync(TestServerUrl);
    }

    /// <summary>
    /// 测试多个task一起启动
    /// </summary>
    /// <returns></returns>
    [Benchmark]
    public async Task MultiTask()
    {
        var tList = new List<Task>();
        var t = _client.GetStringAsync(TestServerUrl);
        for (var i = 0; i < 10000; i++)
        {
            tList.Add(t);
        }

        await Task.WhenAll(tList);
    }
}