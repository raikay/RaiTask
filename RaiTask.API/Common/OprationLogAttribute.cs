using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RaiTask.API.Attributes
{
    /// <summary>
    /// 禁用操作日志
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class OprationLogAttribute : Attribute
    {
    }
}
