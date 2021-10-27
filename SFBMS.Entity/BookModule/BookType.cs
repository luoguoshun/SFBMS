using SFBMS.Entity.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SFBMS.Entity.BookModule
{
    [Table("BookType")]

    public class BookType : BaseEntity
    {
        [Key]
        public int TypeId { get; set; }
        public string TypeName { get; set; }
    }
}
