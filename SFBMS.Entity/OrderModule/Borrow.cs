using SFBMS.Entity.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SFBMS.Entity.OrderModule
{
    [Table("BorrowInfo")]
    public class Borrow : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public int BookId { get; set; }
        public string ClientNo { get; set; }
        /// <summary>
        /// 借阅数量
        /// </summary>
        public int Number { get; set; }
        /// <summary>
        /// 押金
        /// </summary>
        public int Deposit { get; set; }
        /// <summary>
        /// 预期归还日期
        /// </summary>
        public DateTime ExpectedReturnTime { get; set; }
        /// <summary>
        /// 实际归还日期
        /// </summary>
        public DateTime ActualReturnTime { get; set; }
        public DateTime CreateTime { get; set; }
        public ToExamine ToExamine { get; set; }
    }
}
