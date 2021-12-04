using SFBMS.Contracts.BookModule;
using SFBMS.Contracts.MessageModule;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SFBMS.SignalR.Hub
{
    /// <summary>
    /// 定义服务器回调方法(强类型中心)
    /// </summary>
    public interface IChatHub
    {
        /// <summary>
        /// 发送消息给所有用户
        /// </summary>
        /// <param name="user"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        Task ReceiveAllMessage(MessageDTO messagdto);
        /// <summary>
        /// 发送消息给指定用户
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        Task ReceivePrivateMessage(MessageDTO messagdto);
        /// <summary>
        /// 提示异地登入
        /// </summary>
        /// <returns></returns>
        Task Abort(MessageDTO messagdto);
        /// <summary>
        /// 推送书籍订阅统计
        /// </summary>
        /// <returns></returns>
        Task GetStatisticsSubscribe(IList<StatisticsSubscribeDTO> dto);
    }
}
