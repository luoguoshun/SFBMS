using SFBMS.Common.EnumList;
using SFBMS.Common.SiteConfig;
using System;
using System.Collections.Generic;
using System.Text;

namespace SFBMS.Contracts.ClientModule
{
    /// <summary>
    /// 输出模型
    /// </summary>
    public class ClientDTO
    {
        public string ClientNo { get; set; }
        public string Name { get; set; }
        public Sex Sex { get; set; }
        public string SexStr => Sex.ToString();
        /// <summary>
        /// 身份证号
        /// </summary>
        public string IdNumber { get; set; }
        public DateTime BirthDate { get; set; }
        public string Address { get; set; }
        /// <summary>
        /// 联系方式
        /// </summary>
        public string Phone { get; set; }
        public int State { get; set; }
        public string RoleName { get; set; }
        public string HeaderImgSrc { get; set; }
        public string ImageUrl => SiteConfigHelper.GetSectionValue("BaseUrl") + HeaderImgSrc;
        public DateTime CreateTime { get; set; }
    }
    public class ClientOutDTO
    {
        public IEnumerable<ClientDTO> Clients { get; set; }
        public int Count { get; set; }
    }
    /// <summary>
    /// 查询模型
    /// </summary>
    public class SelectClientDTO
    {
        public int Page { get; set; }
        public int Row { get; set; }
        public string Conditions { get; set; }
        public int RoleId { get; set; }
    }
    /// <summary>
    /// 创建用户模型
    /// </summary>
    public class CreateClientDTO
    {
        public string Name { get; set; }
        public Sex Sex { get; set; }
        /// <summary>
        /// 身份证号
        /// </summary>
        public string IdNumber { get; set; }
        public DateTime BirthDate { get; set; }
        public string Address { get; set; }
        /// <summary>
        /// 联系方式
        /// </summary>
        public string Phone { get; set; }
        public string Password { get; set; }
    }
    /// <summary>
    /// 修改模型
    /// </summary>
    public class UpdateClientDTO
    {
        public string ClientNo { get; set; }
        public string Name { get; set; }
        public int Sex { get; set; }
        /// <summary>
        /// 身份证号
        /// </summary>
        public string IdNumber { get; set; }
        public DateTime BirthDate { get; set; }
        public string Address { get; set; }
        /// <summary>
        /// 联系方式
        /// </summary>
        public string Phone { get; set; }
        public int State { get; set; }       
    }
    /// <summary>
    /// 删除模型
    /// </summary>
    public class DeleteClientDTO
    {
        public string[] ClientNos { get; set; }
    }
}
