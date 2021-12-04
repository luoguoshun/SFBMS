using SFBMS.Contracts.SystemModule;
using System.Threading.Tasks;

namespace SFBMS.Service.SystemModule
{
    public interface ILogService
    {
        Task<NLogOutDTO> GetNLogListAsync(SelectNLogDTO dto);
        Task<bool> CreateNLogAsync(CreateNLogDTO dto);
    }
}
