using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Quartz;
using RaiTask.API.Dtos;
using RaiTask.Infrastructure;
using RaiTask.IServices;
using RaiTask.IServices.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RaiTask.API.Controllers
{
    /// <summary>
    /// 任务调度
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class SchedulerController : ControllerBase
    {

        private readonly IJobCenter _service;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="service"></param>
        public SchedulerController(IJobCenter service)
        {
            _service = service;
        }


        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResponseOutput> AddJob([FromBody] ScheduleDto entity)
        {
            return await _service.AddScheduleJobAsync(entity);
        }

        ///// <summary>
        ///// 添加任务
        ///// </summary>
        ///// <param name="entity"></param>
        ///// <returns></returns>
        //[HttpPost("enable")]
        //public async Task<IResponseOutput> enable()
        //{
        //    await _service.StartScheduleAsync();
        //    return ResponseOutput.Ok();
        //}


        /// <summary>
        /// 暂停任务
        /// </summary>
        /// <returns></returns>
        [HttpPost("StopJob")]
        public async Task<IResponseOutput> StopJob([FromBody] JobKey job)
        {
            return await _service.StopOrDelScheduleJobAsync(job.Group, job.Name);
        }

        /// <summary>
        /// 删除任务
        /// </summary> 
        /// <returns></returns>
        [HttpDelete("del")]
        public async Task<IResponseOutput> RemoveJob([FromBody] JobKey job)
        {
            return await _service.StopOrDelScheduleJobAsync(job.Group, job.Name, true);
        }

        /// <summary>
        /// 恢复运行暂停的任务
        /// </summary> 
        /// <returns></returns>
        [HttpPost("ResumeJob")]
        public async Task<IResponseOutput> ResumeJob([FromBody] JobKey job)
        {
            return await _service.ResumeJobAsync(job.Group, job.Name);
        }

        /// <summary>
        /// 查询任务
        /// </summary>
        /// <returns></returns>
        [HttpPost("QueryJob")]
        public async Task<ScheduleDto> QueryJob([FromBody] JobKey job)
        {
            return await _service.QueryJobAsync(job.Group, job.Name);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost("ModifyJob")]
        public async Task<IResponseOutput> ModifyJob([FromBody] ModifyJobInput entity)
        {

            var jobKey = new JobKey(entity.OldScheduleEntity.JobName, entity.OldScheduleEntity.JobGroup);
            var runNumber = await _service.GetRunNumberAsync(jobKey);
            await _service.StopOrDelScheduleJobAsync(entity.OldScheduleEntity.JobGroup, entity.OldScheduleEntity.JobName, true);
            await _service.AddScheduleJobAsync(entity.NewScheduleEntity, runNumber);
            return ResponseOutput.Ok("修改计划任务成功！");
        }

        /// <summary>
        /// 立即执行
        /// </summary>
        /// <param name="job"></param>
        /// <returns></returns>
        [HttpPost("TriggerJob")]
        public async Task<bool> TriggerJob([FromBody] JobKey job)
        {
            await _service.TriggerJobAsync(job);
            return true;
        }

        /// <summary>
        /// 获取job日志
        /// </summary>
        /// <param name="jobKey"></param>
        /// <returns></returns>
        [HttpPost("GetJobLogs")]
        public async Task<List<string>> GetJobLogs([FromBody] JobKey jobKey)
        {
            var logs = await _service.GetJobLogsAsync(jobKey);
            logs?.Reverse();
            return logs;
        }

        /// <summary>
        /// 启动调度
        /// </summary>
        /// <returns></returns>
        [HttpGet("StartSchedule")]
        public async Task<bool> StartSchedule()
        {
            return await _service.StartScheduleAsync();
        }

        /// <summary>
        /// 停止调度
        /// </summary>
        /// <returns></returns>
        [HttpGet("StopSchedule")]
        public async Task<bool> StopSchedule()
        {
            return await _service.StopScheduleAsync();
        }

        /// <summary>
        /// 获取所有任务
        /// </summary>
        /// <returns></returns>
        [HttpGet("Jobs")]
        public  Task<IResponseOutput> GetAllJob()
        {
            return  _service.GetAllJobAsync();
        }

        /// <summary>
        /// 获取所有Job信息（简要信息 - 刷新数据的时候使用）
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAllJobBriefInfo")]
        public async Task<List<JobBriefInfoEntity>> GetAllJobBriefInfo()
        {
            return await _service.GetAllJobBriefInfoAsync();
        }

        /// <summary>
        /// 移除异常信息
        /// </summary>
        /// <param name="jobKey"></param>
        /// <returns></returns>
        [HttpPost("RemoveErrLog")]
        public async Task<bool> RemoveErrLog([FromBody] JobKey jobKey)
        {
            return await _service.RemoveErrLog(jobKey.Group, jobKey.Name);
        }











    }
}
