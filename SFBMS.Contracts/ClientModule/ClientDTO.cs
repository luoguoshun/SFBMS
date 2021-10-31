using SFBMS.Common.EnumList;
using System;
using System.Collections.Generic;
using System.Text;

namespace SFBMS.Contracts.ClientModule
{
    public class ClientDTO
    {
        public int ClientNo { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public Sex Sex { get; set; }
        public string SexStr => Sex.ToString();
        /// <summary>
        /// 身份证号
        /// </summary>
        public string IdNumber { get; set; }
        public string BirthDate { get; set; }
        public string Address { get; set; }
        /// <summary>
        /// 联系方式
        /// </summary>
        public string Phone { get; set; }
        public string Flag { get; set; }
    }

    public class ClientOutDTO
    {
        public IEnumerable<ClientDTO> Clients { get; set; }
        public int Count { get; set; }
    }
    public class SelectClientDTO
    {
        public string Name { get; set; }
        public string Phone { get; set; }
    }

}
