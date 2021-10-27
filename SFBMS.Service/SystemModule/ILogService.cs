using SFBMS.Contracts.LogMolue;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SFBMS.Service.SystemModule
{
   public interface ILogService
    {
      Task<LogOutDTO>  GetLogList();
    }
}
