using SFBMS.Contracts.SystemModule;
using SFBMS.Entity.SystemModule;
using SFBMS.Repository.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SFBMS.Repository.SystemModule
{
    public interface IRoleRepository : IRepository<RoleInfo>
    {
        Task<IList<RoleDTO>> GetRoleListAsync();

    }
}
