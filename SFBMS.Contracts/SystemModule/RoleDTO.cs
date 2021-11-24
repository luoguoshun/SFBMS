using System;
using System.Collections.Generic;
using System.Text;

namespace SFBMS.Contracts.SystemModule
{
    /// <summary>
    /// 输出模型
    /// </summary>
    public class RoleDTO
    {
        public int RoleId { get; set; }
        public string Name { get; set; }
        public string Descripcion { get; set; }
    }
    public class RoleOutDTO
    {
        public IEnumerable<RoleDTO> Roles { get; set; }
        public int CountP { get; set; }
    }
}
