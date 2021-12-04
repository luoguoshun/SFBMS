using SFBMS.Common.EnumList;
using SFBMS.Entity.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SFBMS.Entity.OrderModule
{
    public class Order : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public string BorrowId { get; set; }
        public string ClientNo { get; set; }
        public OrderTypes OrderType { get; set; }
        public PayTypes PayType { get; set; }
        public int Money { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
