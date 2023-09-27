
using Quartz;

namespace AddQuartz.Jobs
{
    /// <summary>
    /// 一个demo
    /// </summary>
    public class HelloJob : IJob
    {
        private readonly ILogger<HelloJob> _logger;

        /// <summary>
        /// 依赖注入
        /// </summary>
        /// <param name="logger"></param>
        public HelloJob(ILogger<HelloJob> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// job的执行入口
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation($"{DateTime.Now},Greetings from {nameof(HelloJob)}!");
            await Task.Yield();
        }
    }
}
