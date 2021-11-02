using SFBMS.Entity.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SFBMS.Entity.BookModule
{
    [Table("BookInfo")]
    public class Book : BaseEntity
    {
        /// <summary>
        /// 书籍编号
        /// </summary>
        [Key]
        public int Id { get; set; }
        public int TypeId { get; set; }
        public string BookName { get; set; }
        public string Author { get; set; }
        public string Press { get; set; }
        public DateTime PublicationDate { get; set; }
        public int Price { get; set; }
        /// <summary>
        /// 库存量
        /// </summary>
        public int Inventory { get; set; }
        public string Descripcion { get; set; }
        /// <summary>
        /// 封面地址
        /// </summary>
        public string ImageSrc { get; set; }
        public DateTime CreateTime { get; set; }        
    }
}
