using SFBMS.Entity.Base;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SFBMS.Entity.BookModule
{
    [Table("Subscribe")]
    public class Subscribe : BaseEntity
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public string ClientNo { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
