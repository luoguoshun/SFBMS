using SFBMS.Contracts.BookModule;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SFBMS.Service.BookModule
{
   public interface ISubscribeService
    {
        Task<IEnumerable<SubscribeDTO>> GetSubscribeListAsync();
        Task<IList<StatisticsSubscribeDTO>> StatisticsAsync();
    }
}
