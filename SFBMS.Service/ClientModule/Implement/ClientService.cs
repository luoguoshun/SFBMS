using SFBMS.Contracts.ClientModule;
using SFBMS.Repository.ClientModule;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SFBMS.Service.ClientModule.Implement
{
    public class ClientService : IClientService
    {
        public readonly IClientRepository _clientRepository;
        public ClientService(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }
        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<ClientOutDTO> GetClientListAsync(SelectClientDTO dto)
        {
            return await _clientRepository.GetClientListAsync(dto);
        }
    }
}
