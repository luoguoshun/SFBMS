using SFBMS.Contracts.ClientModule;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SFBMS.Service.ClientModule
{
   public interface IClientService
    {
        Task<ClientOutDTO> GetClientListAsync(SelectClientDTO dto);
    }
}
