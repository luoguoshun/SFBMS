using SFBMS.Contracts.SystemModule;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SFBMS.Service.SystemModule
{
    public interface IRoleService
    {
        Task<IList<RoleDTO>> GetRoleListAsync();
    }
}
