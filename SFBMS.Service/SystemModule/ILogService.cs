using SFBMS.Contracts.LogModule;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SFBMS.Service.SystemModule
{
   public interface ILogService
    {
      Task<LogOutDTO>  GetLogListAsync(SelectLogDTO dto);
    }
}
