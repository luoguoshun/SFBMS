using SFBMS.Contracts.SystemModule;
using SFBMS.Repository.SystemModule;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SFBMS.Service.SystemModule.Implement
{
    public class RoleService : IRoleService
    {
        public IRoleRepository _roleRepository;
        public RoleService(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }
        /// <summary>
        /// 获取角色列表
        /// </summary>
        /// <returns></returns>
        public async Task<IList<RoleDTO>> GetRoleListAsync()
        {
            return await _roleRepository.GetRoleListAsync();
        }
    }
}
