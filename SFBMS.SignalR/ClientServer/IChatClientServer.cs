using System;
using System.Collections.Generic;
using System.Text;

namespace SFBMS.SignalR.ClientServer
{
    public interface IChatClientServer
    {
        /// <summary>
        /// 添加信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="connectionId"></param>
        void AddUser(string userId, string connectionId);
        /// <summary>
        /// 移除信息
        /// </summary>
        /// <param name="id"></param>
        void RemoveUser(string userId);
        /// <summary>
        /// 修改用户连接ID
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="connectionId"></param>
        void UpdateUser(string userId, string connectionId);
        /// <summary>
        /// 获取用户连接ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        string GetUserConnectionId(string userId);
    }
}
