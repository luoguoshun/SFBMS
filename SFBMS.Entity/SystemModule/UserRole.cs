using SFBMS.Entity.ClientModule;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SFBMS.Entity.SystemModule
{
    [Table("User_Role")]
    public class UserRole
    {  
        public int RoleId { get; set; }
        public string UserId { get; set; }  
        public RoleInfo Role { get; set; }
        public Client Client { get;set; }
    }
}
