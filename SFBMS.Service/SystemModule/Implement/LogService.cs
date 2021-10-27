using SFBMS.Contracts.LogMolue;
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

        public Task<LogOutDTO> GetLogList()
        {
            throw new NotImplementedException();
        }
    }
}
