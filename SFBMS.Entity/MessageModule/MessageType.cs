using SFBMS.Entity.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SFBMS.Entity.MessageModule
{
    [Table("MessageType")]
    public class MessageType : BaseEntity
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Key]
        public int TypeId { get; set; }
        /// <summary>
        /// 类型说明
        /// </summary>
        public string TypeName { get; set; }
    }
}
