using RaiTask.Infrastructure;
using RaiTask.Infrastructure.Dtos;
using RaiTask.IServices.Dtos;
using RaiTask.Repository.Entities.Base;
using System.Threading.Tasks;

namespace RaiTask.IServices
{
    /// <summary>
    /// 日志
    /// </summary>
    public interface ILoginLogService
    {
        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> PageAsync(PageInput<LoginLogEntity> input);
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IResponseOutput> AddAsync(LoginLogAddInput input);
    }
}
