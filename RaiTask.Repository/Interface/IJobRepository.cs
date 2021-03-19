using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RaiTask.Repository.Interface
{

    public interface IJobRepository
    {
        /// <summary>
        /// 初始化表结构
        /// </summary>
        /// <returns></returns>
        Task<int> InitTable();
        Task<bool> RemoveErrLogAsync(string jobGroup, string jobName);
    }
}
