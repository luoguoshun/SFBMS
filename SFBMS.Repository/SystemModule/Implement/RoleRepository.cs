using SFBMS.Contracts.SystemModule;
using SFBMS.Entity.Context;
using SFBMS.Entity.SystemModule;
using SFBMS.Repository.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFBMS.Repository.SystemModule.Implement
{
    public class RoleRepository : Repository<RoleInfo>, IRoleRepository
    {
        public RoleRepository(SFBMSContext dbContext) : base(dbContext)
        {

        }

        public async Task<IList<RoleDTO>> GetRoleListAsync()
        {
            var data = await GetAllAsync();
            return data.Select(item => new RoleDTO {
                                           RoleId = item.RoleId,
                                           Name=item.Name,
                                           Descripcion=item.Descripcion }).ToList();
        }
    }
}
