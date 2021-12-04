using SFBMS.Contracts.SystemModule;
using SFBMS.Entity.SystemModule;
using SFBMS.Repository.SystemModule;
using System;
using System.Threading.Tasks;

namespace SFBMS.Service.SystemModule.Implement
{
    public class LogService : ILogService
    {
        public INLogRepository _logRepository;
        public LogService(INLogRepository logRepository)
        {
            _logRepository = logRepository;
        }
        /// <summary>
        /// 获取日志列表
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<NLogOutDTO> GetNLogListAsync(SelectNLogDTO dto)
        {
            return await _logRepository.GetLogListAsync(dto);
        }
        /// <summary>
        /// 添加日志
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<bool> CreateNLogAsync(CreateNLogDTO dto)
        {
            NLogInfo log = new NLogInfo
            {
                MachineId = dto.MachineId,
                Origin = dto.Origin,
                RouteInfo = dto.RouteInfo,
                Level = dto.Level,
                Message = dto.Message,
                Detail = dto.Detail,
                Date = DateTime.Now
            };
            await _logRepository.AddEntityAsync(log);
            return await _logRepository.SaveChangeAsync();
        }
    }
}
