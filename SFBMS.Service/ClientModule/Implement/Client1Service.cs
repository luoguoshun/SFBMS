using SFBMS.Contracts.ClientModule;
using SFBMS.Repository.ClientModule;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SFBMS.Service.ClientModule.Implement
{
    public class Client1Service : IClientService
    {
        public readonly IClientRepository _clientRepository;
        public Client1Service(IClientRepository clientRepository)
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
            List<Entity.ClientModule.Client> data = await _clientRepository.GetAllAsync();
            var t= new ClientOutDTO();
            return t;
        }
    }
}
