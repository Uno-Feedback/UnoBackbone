﻿using Quartz;
using Uno.Api.Quartz.Jobs;

namespace Uno.Api.Quartz.Config;

public static class QuartzRegisteration
{
    public static IServiceCollection RegisterQuartzServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
        services.AddQuartz(q =>
        {
            q.UseMicrosoftDependencyInjectionJobFactory();
            var jobkey = new JobKey(nameof(SendIssueJob));
            q.AddJob<SendIssueJob>(opts => opts.WithIdentity(jobkey.Name));
            q.AddTrigger(opts => opts.ForJob(jobkey)
                                .WithIdentity($"{jobkey.Name}.trigger")
                                .WithSimpleSchedule(x =>
                                    x.WithIntervalInMinutes(5)
                                    .RepeatForever()));
        });

        return services;
    }
}