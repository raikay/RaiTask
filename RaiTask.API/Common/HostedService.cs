using Microsoft.Extensions.Hosting;
using RaiTask.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RaiTask.API.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class HostedService : IHostedService
    {
        private readonly IJobCenter schedulerCenter;
        public HostedService(IJobCenter schedulerCenter)
        {
            this.schedulerCenter = schedulerCenter;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            //开启调度器
            await schedulerCenter.StartScheduleAsync();


        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
