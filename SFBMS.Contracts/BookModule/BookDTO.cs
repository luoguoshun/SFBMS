using Microsoft.AspNetCore.Http;
using SFBMS.Common.SiteConfig;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SFBMS.Contracts.BookModule
{
    /// <summary>
    /// 输出模型
    /// </summary>
    public class BookDTO
    {
        public int Id { get; set; }
        public string BookName { get; set; }
        public string TypeName { get; set; }
        public int TypeId { get; set; }
        public string Author { get; set; }
        public string Press { get; set; }
        public DateTime PublicationDate { get; set; }
        public float Price { get; set; }
        /// <summary>
        /// 库存量
        /// </summary>
        public int Inventory { get; set; }
        public string Descripcion { get; set; }
        /// <summary>
        /// 封面地址
        /// </summary>
        public string CoverImgSrc { get; set; }
        public string ImageUrl => SiteConfigHelper.GetSectionValue("BaseUrl") + CoverImgSrc;
        public int State { get; set; }
        public DateTime CreateTime { get; set; }

    }
    public class BookOutDTO
    {
        public IEnumerable<BookDTO> Books { get; set; }
        public int Count { get; set; }
    }
    /// <summary>
    /// 查询模型
    /// </summary>
    public class SelectBookDTO
    {
        public int Row { get; set; }
        public int Page { get; set; }
        public string Conditions { get; set; }
        public int TypeId { get; set; }
        public string[] PublicationDates { get; set; }      
    }
    /// <summary>
    /// 删除模型
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
        public float Price { get; set; }
        /// <summary>
        /// 库存量
        /// </summary>
        public int Inventory { get; set; }
        public string Descripcion { get; set; }
        public int State { get; set; }
    }
    public class BookTypeDTO
    {
        public int TypeId { get; set; }
        public string TypeName { get; set; }
    }
    /// <summary>
    /// 添加模型
    /// </summary>
    public class CreateBookDTO
    {
        public string BookName { get; set; }
        public int TypeId { get; set; }
        public string Author { get; set; }
        public string Press { get; set; }
        public DateTime PublicationDate { get; set; }
        public float Price { get; set; }
        /// <summary>
        /// 库存量
        /// </summary>
        public int Inventory { get; set; }
        public string Descripcion { get; set; }
        /// <summary>
        /// 封面地址
        /// </summary>
        public string ImageSrc { get; set; }
    }
    /// <summary>
    /// 导入数据模型
    /// </summary>
    public class ImportBookDTO
    {
        public string BookName { get; set; }
        public string TypeName { get; set; }
        public string Author { get; set; }
        public string Press { get; set; }
        public DateTime PublicationDate { get; set; }      
        /// <summary>
        /// 数量
        /// </summary>
        public int Inventory { get; set; }
        public float Price { get; set; }
        /// <summary>
        /// 字数
        /// </summary>
        public int WordNumber { get; set; }
        public string Descripcion { get; set; }
    }
}
