using Quartz;
using RaiTask.Infrastructure;
using RaiTask.IServices.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RaiTask.IServices
{
    public interface IJobCenter : IService
    {
        Task<bool> StartScheduleAsync();
        Task<IResponseOutput> AddScheduleJobAsync(ScheduleDto entity, long? runNumber = null);

        Task<IResponseOutput> StopOrDelScheduleJobAsync(string jobGroup, string jobName, bool isDelete = false);


        Task<IResponseOutput> ResumeJobAsync(string jobGroup, string jobName);

        Task<ScheduleDto> QueryJobAsync(string jobGroup, string jobName);

        Task<bool> TriggerJobAsync(JobKey jobKey);
        Task<List<string>> GetJobLogsAsync(JobKey jobKey);

        Task<long> GetRunNumberAsync(JobKey jobKey);

        Task<IResponseOutput> GetAllJobAsync();

        Task<bool> RemoveErrLog(string jobGroup, string jobName);

        Task<List<JobBriefInfoEntity>> GetAllJobBriefInfoAsync();

        Task<bool> StopScheduleAsync();








    }
}
