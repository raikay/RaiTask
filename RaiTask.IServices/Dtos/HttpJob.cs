using Newtonsoft.Json;
using Quartz;
using RaiTask.Common.Helper;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace RaiTask.IServices.Dtos
{
    public abstract class LogModel
    {
        /// <summary>
        /// 开始执行时间
        /// </summary>
        [JsonIgnore]
        public string BeginTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        [JsonIgnore]
        public string EndTime { get; set; }
        /// <summary>
        /// 耗时（秒）
        /// </summary>
        [JsonIgnore]
        public string ExecuteTime { get; set; }
        /// <summary>
        /// 任务名称
        /// </summary>
        public string JobName { get; set; }
        /// <summary>
        /// 结果
        /// </summary>
        public string Result { get; set; }
        /// <summary>
        /// 异常消息
        /// </summary>
        public string ErrorMsg { get; set; }
    }
    public class LogUrlModel : LogModel
    {
        /// <summary>
        /// 请求地址
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 请求类型
        /// </summary>
        public string RequestType { get; set; }
        /// <summary>
        /// 请求参数
        /// </summary>
        public string Parameters { get; set; }
    }

    /// <summary>
    /// Job任务结果
    /// </summary>
    public class HttpResultModel
    {
        /// <summary>
        /// 请求是否成功
        /// </summary>
        public bool IsSuccess { get; set; }
        /// <summary>
        /// 异常消息
        /// </summary>
        public string ErrorMsg { get; set; }
    }
    public class HttpJob : JobBase<LogUrlModel>, IJob
    {
        public HttpJob() : base(new LogUrlModel())
        { }

        public override async Task NextExecute(IJobExecutionContext context)
        {
            //获取相关参数
            var requestUrl = context.JobDetail.JobDataMap.GetString(JobConstant.REQUESTURL)?.Trim();
            requestUrl = requestUrl?.IndexOf("http") == 0 ? requestUrl : "http://" + requestUrl;
            var requestParameters = context.JobDetail.JobDataMap.GetString(JobConstant.REQUESTPARAMETERS);
            var headersString = context.JobDetail.JobDataMap.GetString(JobConstant.HEADERS);
            var headers = headersString != null ? JsonConvert.DeserializeObject<Dictionary<string, string>>(headersString?.Trim()) : null;
            var requestType = (RequestTypeEnum)int.Parse(context.JobDetail.JobDataMap.GetString(JobConstant.REQUESTTYPE));


            LogInfo.Url = requestUrl;
            LogInfo.RequestType = requestType.ToString();
            LogInfo.Parameters = requestParameters;

            HttpResponseMessage response = new HttpResponseMessage();
            var http = HttpHelper.Instance;
            switch (requestType)
            {
                case RequestTypeEnum.Get:
                    response = await http.GetAsync(requestUrl, headers);
                    break;
                case RequestTypeEnum.Post:
                    response = await http.PostAsync(requestUrl, requestParameters, headers);
                    break;
                case RequestTypeEnum.Put:
                    response = await http.PutAsync(requestUrl, requestParameters, headers);
                    break;
                case RequestTypeEnum.Delete:
                    response = await http.DeleteAsync(requestUrl, headers);
                    break;
            }
            var result = HttpUtility.HtmlEncode(await response.Content.ReadAsStringAsync());
            LogInfo.Result = $"<span class='result'>{result}</span>";
            if (!response.IsSuccessStatusCode)
            {
                LogInfo.ErrorMsg = $"<span class='error'>{result}</span>";
                await ErrorAsync(LogInfo.JobName, new Exception(result), JsonConvert.SerializeObject(LogInfo), MailLevel);
                context.JobDetail.JobDataMap[JobConstant.EXCEPTION] = $"<div class='err-time'>{LogInfo.BeginTime}</div>{JsonConvert.SerializeObject(LogInfo)}";
            }
            else
            {
                try
                {
                    //这里需要和请求方约定好返回结果约定为HttpResultModel模型
                    var httpResult = JsonConvert.DeserializeObject<HttpResultModel>(HttpUtility.HtmlDecode(result));
                    if (!httpResult.IsSuccess)
                    {
                        LogInfo.ErrorMsg = $"<span class='error'>{httpResult.ErrorMsg}</span>";
                        await ErrorAsync(LogInfo.JobName, new Exception(httpResult.ErrorMsg), JsonConvert.SerializeObject(LogInfo), MailLevel);
                        context.JobDetail.JobDataMap[JobConstant.EXCEPTION] = $"<div class='err-time'>{LogInfo.BeginTime}</div>{JsonConvert.SerializeObject(LogInfo)}";
                    }
                    else
                        await InformationAsync(LogInfo.JobName, JsonConvert.SerializeObject(LogInfo), MailLevel);
                }
                catch (Exception)
                {
                    await InformationAsync(LogInfo.JobName, JsonConvert.SerializeObject(LogInfo), MailLevel);
                }
            }
        }
    }
}
