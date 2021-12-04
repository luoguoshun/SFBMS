using System;
using System.Collections.Generic;
using System.Text;

namespace SFBMS.Contracts.BookModule
{
    /// <summary>
    /// 输出模型
    /// </summary>
    public class SubscribeDTO
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public string BookName { get; set; }
        public string ClientNo { get; set; }
        public string ClientName { get; set; }
        public DateTime CreateTime { get; set; }
    }
    /// <summary>
    /// 书籍订阅统计模型(填充图标数据)
    /// </summary>
    public class StatisticsSubscribeDTO
    {
        public int value { get; set; }
        public string name { get; set; }
    }
}
