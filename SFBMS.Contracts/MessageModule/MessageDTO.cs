using System;
using System.Collections.Generic;
using System.Text;

namespace SFBMS.Contracts.MessageModule
{
    public class MessageDTO
    {
        public string Id { get; set; }
        public string SenderId { get; set; }
        public string ReceiverId { get; set; }
        public string Content { get; set; }
        public DateTime CreateTime { get; set; }
        public int MessageType { get; set; }
    }
}
