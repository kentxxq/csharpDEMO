using AddQuartz.Jobs;
using Quartz;

namespace AddQuartz;

public static class MyQuartz
{
    public static void AddMyQuartz(WebApplicationBuilder builder)
    {
        // 启用quartz定时器
        builder.Services.Configure<QuartzOptions>(builder.Configuration.GetSection("Quartz"));
        builder.Services.AddQuartz(q =>
        {
            q.UseMicrosoftDependencyInjectionJobFactory();

            q.ScheduleJob<HelloJob>(trigger =>
            {
                trigger
                    .WithIdentity("hellojob", "group1")
                    .StartNow()
                    .WithSimpleSchedule(b =>
                    {
                        b.WithIntervalInMinutes(5)
                            .RepeatForever();
                    })
                    .WithDescription("hellojob task");
            });


            q.ScheduleJob<DataJob>(trigger =>
            {
                trigger
                    .WithIdentity("datajob", "group2")
                    .UsingJobData("data", "datajob-data")
                    .StartNow()
                    .WithCronSchedule("5 * * * * ?")
                    .WithDescription("datajob task");
            });

            //var jobKey = new JobKey("awesome job", "awesome group");
            //q.AddJob<HelloJob>(jobKey, j => j
            //    .WithDescription("my awesome job")
            //);

            //q.AddTrigger(t => t
            //    .WithIdentity("Simple Trigger")
            //    .ForJob(jobKey)
            //    .StartNow()
            //    .WithSimpleSchedule(x => x.WithInterval(TimeSpan.FromSeconds(10)).RepeatForever())
            //    .WithDescription("my awesome simple trigger")
            //);

            //q.AddTrigger(t => t
            //    .WithIdentity("Cron Trigger")
            //    .ForJob(jobKey)
            //    .StartAt(DateBuilder.EvenSecondDate(DateTimeOffset.UtcNow.AddSeconds(3)))
            //    .WithCronSchedule("0/3 * * * * ?")
            //    .WithDescription("my awesome cron trigger")
            //);
        });
        builder.Services.AddQuartzServer(option => { option.WaitForJobsToComplete = true; });
    }
}