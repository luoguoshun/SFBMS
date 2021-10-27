using SFBMS.Common.EnumList;
using SFBMS.Entity.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SFBMS.Entity.SystemModule
{
    [Table("AdminInfo")]
   public class AdminInfo:BaseEntity
    {
        [Key]
        public string AdminNo { get; set; }
        public string Name { get; set; }
        public string PassWord { get; set; }
        public Sex Sex { get; set; }

    }
}
