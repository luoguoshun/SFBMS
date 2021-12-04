using SFBMS.Common.EnumList;
using SFBMS.Entity.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SFBMS.Entity.OrderModule
{
    [Table("ToExamine")]
    public class ToExamine : BaseEntity
    {
        [Key]
        public string Id { get; set; }
        public int BorrowId { get; set; }
        public string AdminNo { get; set; }
        public string ClientNo { get; set; }
        public ExamineType State { get; set; }
        public DateTime CreateTime { get; set; }
        public Borrow Borrow { get; set; }
    }
}
