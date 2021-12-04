using SFBMS.Entity.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SFBMS.Entity.MessageModule
{
    /// <summary>
    /// 聊天信息记录表
    /// </summary>
    [Table("MessageInfo")]
    public class Message : BaseEntity
    {
        [Key]
        public string Id { get; set; }
        public string SenderId { get; set; }
        public string ReceiverId { get; set; }
        [MaxLength(500)]
        public string Content { get; set; }
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 信息类型外键 
        /// </summary>
        ///[Column("MessageTypeId")]
        public int MessageTypeId { get; set; }
        #region 备注：属性满足以下要求则配置为外键
        // 如果依赖实体包含一个名称与以下模式之一匹配的属性，则它将配置为外键：
        //<导航属性名称><主键属性名称>
        //<导航属性名称>Id
        //<主体实体名称><主键属性名称>
        //<主体实体名称>Id
        #endregion
    }
}
