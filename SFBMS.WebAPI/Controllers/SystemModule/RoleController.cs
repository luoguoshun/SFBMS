using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OHEXML.Infrastructure.Attributes;
using SFBMS.Common.EnumList;
using SFBMS.Service.SystemModule;
using SFBMS.WebAPI.Controllers.Base;

namespace SFBMS.WebAPI.Controllers.SystemModule
{
    public class RoleController : BaseController
    {
        public IRoleService _roleService;
        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }
        /// <summary>
        /// 获取角色列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AppAuthorize(AppTypes.Background)]
        public async Task<IActionResult> GetRoleList()
        {
            var result =await _roleService.GetRoleListAsync();
            return JsonSuccess(result);
        }
    }
}
