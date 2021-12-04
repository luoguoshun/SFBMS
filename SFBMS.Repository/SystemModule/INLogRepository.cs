using SFBMS.Contracts.SystemModule;
using SFBMS.Entity.SystemModule;
using SFBMS.Repository.Base;
using System.Threading.Tasks;

namespace SFBMS.Repository.SystemModule
{
    public interface INLogRepository: IRepository<NLogInfo>
    {
        Task<NLogOutDTO> GetLogListAsync(SelectNLogDTO dto);
    }
}
