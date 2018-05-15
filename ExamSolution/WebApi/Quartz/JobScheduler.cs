using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Quartz;
using Quartz.Impl;

namespace WebApi.Quartz
{
    public class JobScheduler

    {

        public static async void Start()

        {

            ISchedulerFactory schedulerFactory = new StdSchedulerFactory();
            IScheduler scheduler = await schedulerFactory.GetScheduler();
            await scheduler.Start();

            IJobDetail job = JobBuilder.Create<IDGJob>().Build();

            ITrigger trigger = TriggerBuilder.Create()

                .WithIdentity("IDGJob", "IDG")
                //Trigger that fires every minute
                .WithCronSchedule("0 0/1 * * * ?")

                .StartAt(DateTime.UtcNow)

                .WithPriority(1)

                .Build();

            await scheduler.ScheduleJob(job, trigger);

        }

    }
}
