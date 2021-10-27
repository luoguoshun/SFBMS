using System;
using System.Collections.Generic;
using System.Text;

namespace SFBMS.Contracts.LogMolue
{
    public class LogDTO
    {
        public int LogId { get; set; }
        public DateTime Date { get; set; }
        public string Origin { get; set; }
        public string Level { get; set; }
        public string Message { get; set; }
        public string Detail { get; set; }
    }

    /// <summary>
    /// 输出模型
    /// </summary>
    public class LogOutDTO
    {
        public IEnumerable<LogDTO> Logs { get; set; }
        public int Count { get; set; }
    }
}
