using System;
using System.Threading;

using Quartz;
using Quartz.Impl;

namespace ConsoleApplication1
{
	public class Program
	{
		private static void Main(string[] args)
		{
			IScheduler _scheduler = null;

			try
			{
				Common.Logging.LogManager.Adapter = new Common.Logging.Simple.ConsoleOutLoggerFactoryAdapter
					{
						Level = Common.Logging.LogLevel.Info
					};

				_scheduler = StdSchedulerFactory.GetDefaultScheduler();
				_scheduler.Start();

				var _job = JobBuilder.Create<HelloJob>().WithIdentity("FirstJob").Build();

				var _trigger = GetTrigger();

				_scheduler.ScheduleJob(_job, _trigger);

				// Some sleep to show what's happening
				Thread.Sleep(TimeSpan.FromSeconds(60));
			}
			catch (SchedulerException se)
			{
				Console.WriteLine(se);
			}
			finally
			{
				_scheduler.Shutdown();
			}
			Console.Read();
		}


		private static ITrigger GetTrigger()
		{
			// Simple trigger
			//var _trigger = TriggerBuilder.Create()
			//	.WithIdentity("firstTrigger")
			//	.StartNow()
			//	.WithSimpleSchedule(builder => builder.WithIntervalInSeconds(3).RepeatForever())
			//	.Build();

			// cron trigger, every 2 seconds
			var _trigger = TriggerBuilder.Create()
					.WithIdentity("trigger3", "group1")
					.WithCronSchedule("0/2 * * * * ?")
					.Build();

			return _trigger;
		}
	}



	public class HelloJob :IJob
	{
		public void Execute(IJobExecutionContext context)
		{
			Console.WriteLine("HelloJob");
		}
	}
}