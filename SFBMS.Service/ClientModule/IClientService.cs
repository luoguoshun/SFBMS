using Microsoft.AspNetCore.Http;
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
        Task<bool> CreateClientAsync(CreateClientDTO dto);
        Task<bool> UpdateSectionClientAsync(UpdateClientDTO dto);
        Task<bool> DeleteClientsAsync(string[] clientNos);
        Task<(bool,string)> UpdateAllClientAsync(IFormFile file, UpdateClientDTO dto);
        Task<(bool, string)> SaveImageAsync(IFormFile imageFile);
    }
}
