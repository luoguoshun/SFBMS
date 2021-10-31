using SFBMS.Contracts.ClientModule;
using SFBMS.Entity.ClientModule;
using SFBMS.Repository.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SFBMS.Repository.ClientModule
{
   public interface IClientRepository:IRepository<Client>
    {
        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<ClientOutDTO> GetClientList(SelectClientDTO dto);
    }
}
