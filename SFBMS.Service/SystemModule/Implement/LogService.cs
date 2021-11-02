using SFBMS.Contracts.LogModule;
using SFBMS.Repository.SystemModule;
using System;
using System.Threading.Tasks;

namespace SFBMS.Service.SystemModule.Implement
{
    public class LogService : ILogService
    {
        public ILogRepository _logRepository;
        public LogService(ILogRepository logRepository)
        {
            _logRepository = logRepository;
        }
        /// <summary>
        /// 获取日志列表
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<LogOutDTO> GetLogListAsync(SelectLogDTO dto)
        {
            return await _logRepository.GetLogListAsync(dto);
        }
    }
}
