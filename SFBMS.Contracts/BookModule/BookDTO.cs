using System;
using System.Collections.Generic;
using System.Text;

namespace SFBMS.Contracts.BookModule
{
    public class BookDTO
    {
        public int Id { get; set; }
        public string BookName { get; set; }
        public string TypeName { get; set; }
        public int TypeId { get; set; }
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
    public class BookOutDTO
    {
        public IEnumerable<BookDTO> books { get; set; }
        public int Count { get; set; }
    }
    /// <summary>
    /// 查询模型
    /// </summary>
    public class SelectBookDTO
    {
        public string BookName { get; set; }
        public string Author { get; set; }
        public int TypeId { get; set; }
        public string[] PublicationDates { get; set; }
    }
    /// <summary>
    /// 删除实体
    /// </summary>
    public class DeleteBookDTO
    {
        public int[] BookIds { get; set; }
    }
    /// <summary>
    /// 修改模型
    /// </summary>
    public class UpdateBookDTO
    {
        public int Id { get; set; }
        public string BookName { get; set; }
        public int TypeId { get; set; }
        public DateTime PublicationDate { get; set; }
        public int Price { get; set; }
        /// <summary>
        /// 库存量
        /// </summary>
        public int Inventory { get; set; }
        public string Descripcion { get; set; }
    }
    public class BookTypeDTO
    {
        public int TypeId { get; set; }
        public string TypeName { get; set; }
    }
}
