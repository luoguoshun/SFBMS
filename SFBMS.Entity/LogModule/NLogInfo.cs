using SFBMS.Entity.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SFBMS.Entity.LogModule
{
    [Table("NLogInfo")]
    public class NLogInfo:BaseEntity
    {
        [Key]
        public int LogId { get; set; }
        public DateTime Date { get; set; }
        public string Origin { get; set; }
        public string Level { get; set; }
        [MaxLength(500)]
        public string Message { get; set; }
        [MaxLength(500)]
        public string Detail { get; set; }
    }
}
