using SFBMS.Contracts.LogModule;
using SFBMS.Entity.LogModule;
using SFBMS.Repository.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SFBMS.Repository.SystemModule
{
    public interface ILogRepository: IRepository<NLogInfo>
    {
        Task<LogOutDTO> GetLogListAsync(SelectLogDTO dto);
    }
}
