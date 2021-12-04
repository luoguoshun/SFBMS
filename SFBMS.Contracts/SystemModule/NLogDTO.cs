using System;
using System.Collections.Generic;
using System.Text;

namespace SFBMS.Contracts.SystemModule
{
    /// <summary>
    /// 输出模型
    /// </summary>
    public class NLogDTO
    {
        public int LogId { get; set; }
        public string MachineId { get; set; }
        public string Origin { get; set; }
        public string RouteInfo { get; set; }
        public string Level { get; set; }
        public string Message { get; set; }
        public string Detail { get; set; }
        public DateTime Date { get; set; }
    } 
    public class NLogOutDTO
    {
        public IEnumerable<NLogDTO> Logs { get; set; }
        public int Count { get; set; }
    }
    /// <summary>
    /// 查询模型
    /// </summary>
    public class SelectNLogDTO
    {
        public int Page { get; set; }
        public int Row { get; set; }
        public DateTime[] Dates { get; set; }
        public string Level { get; set; }
        public string Message { get; set; }
    }
    /// <summary>
    /// 添加模型
    /// </summary>
    public class CreateNLogDTO
    {
        public string MachineId { get; set; }
        public string Origin { get; set; }
        public string RouteInfo { get; set; }
        public string Level { get; set; }
        public string Message { get; set; }
        public string Detail { get; set; }
    }
}
