using Fanex.MailDelivery.Client.Jobs;
using Fanex.MailDelivery.Core.Models;
using Fanex.WCLeague.Business.Services;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace Fanex.WCLeague.Service.Services
{
    public class ExecuteWC2025USAService : JobBase
    {
        protected override JobMessage Run(Schedule schedule, IJobExecutionContext jobContext)
        {
            var services = ServiceCollectionExtensions.Services;
            var serviceProvider = services.BuildServiceProvider();

            var mainService = serviceProvider.GetService<MainService>();
            var result = mainService.Execute("WC2025USA");
            return new JobMessage
            {
                JobStatus = result.JobStatus,
                Schedule = schedule,
                Mail = result.Mail
            };
        }
    }
}