using SFBMS.Contracts.BookModule;
using SFBMS.Entity.BookModule;
using SFBMS.Repository.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SFBMS.Repository.BookModule
{
    public interface ISubscribeRepository : IRepository<Subscribe>
    {
        Task<IEnumerable<SubscribeDTO>> GetSubscribeListAsync();
        Task<IList<StatisticsSubscribeDTO>> StatisticsAsync();
    }
}
