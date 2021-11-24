using SFBMS.Common.EnumList;
using SFBMS.Entity.Base;
using SFBMS.Entity.SystemModule;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SFBMS.Entity.ClientModule
{
    [Table("ClientInfo")]
    public class Client : BaseEntity
    {
        [Key]
        public string ClientNo { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public Sex Sex { get; set; }
        /// <summary>
        /// 身份证号
        /// </summary>
        public string IdNumber { get; set; }
        public DateTime BirthDate { get; set; }
        public string Address { get; set; }
        /// <summary>
        /// 联系方式
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 是否开启用户 1开启 2关闭
        /// </summary>
        public int State { get; set; }
        public DateTime CreateTime { get; set; }
        public string HeaderImgSrc { get; set; }
        public List<UserRole> Roles { get; set; }
    }
}
