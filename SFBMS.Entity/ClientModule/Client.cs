using SFBMS.Common.EnumList;
using SFBMS.Entity.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SFBMS.Entity.ClientModule
{
    [Table("ClientInfo")]
    public class Client : BaseEntity
    {
        [Key]
        public int ClientNo { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public Sex Sex { get; set; }
        /// <summary>
        /// 学号
        /// </summary>
        public string IdNumber { get; set; }
        public string BirthDate { get; set; }
        public string Address { get; set; }
        /// <summary>
        /// 联系方式
        /// </summary>
        public string Phone { get; set; }
        public string Flag { get; set; }
    }
}
