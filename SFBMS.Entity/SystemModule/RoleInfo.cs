using SFBMS.Entity.Base;
using SFBMS.Entity.ClientModule;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SFBMS.Entity.SystemModule
{
    [Table("RoleInfo")]
    public class RoleInfo : BaseEntity
    {
        [Key]
        public int RoleId { get; set; }
        public string Name { get; set; }
        public string Descripcion { get; set; }
        public List<UserRole> Roles { get; set; }
    }
}
