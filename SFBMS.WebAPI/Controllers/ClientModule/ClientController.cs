using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OHEXML.Infrastructure.Attributes;
using SFBMS.Common.EnumList;
using SFBMS.Contracts.ClientModule;
using SFBMS.Service.ClientModule;
using SFBMS.WebAPI.Controllers.Base;

namespace SFBMS.WebAPI.Controllers.ClientModule
{
    public class ClientController : BaseController
    {
        public readonly IClientService _clientService;
        public ClientController(IClientService clientService)
        {
            _clientService = clientService;
        }
        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [AppAuthorize(AppTypes.Background)]
        public async Task<IActionResult> GetClientList(SelectClientDTO dto)
        {
            var clients = await _clientService.GetClientListAsync(dto);
            if (clients is null)
            {
                return JsonFailt("");
            }
            return JsonSuccess("", clients);
        }
    }
}
