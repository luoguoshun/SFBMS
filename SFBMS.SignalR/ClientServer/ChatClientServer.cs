using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SFBMS.SignalR.ClientServer
{
    /// <summary>
    /// 防止使用 magic 字符串导致的问题，因为 Hub<T> 只能提供对 接口中定义的方法的访问。
    /// </summary>
    public class ChatClientServer : IChatClientServer
    {
        /// <summary>
        /// 跟踪用户
        /// </summary>
        private readonly List<HubClientDTO> Users;
        private readonly object _obj;
        public ChatClientServer()
        {
            Users = new List<HubClientDTO>();
            _obj = new object();
        }

        /// <summary>
        /// 添加信息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="connectionId"></param>
        public void AddUser(string userId, string connectionId)
        {
            //持有 lock 时持有lock的线程可以再次获取并释放 lock。阻止任何其他线程获取 lock 并等待释放 lock。
            lock (_obj)
            {
                Users.Add(new HubClientDTO
                {
                    UserID = userId,
                    ConnectionId = connectionId,
                });
            }
        }
        /// <summary>
        /// 移除信息
        /// </summary>
        /// <param name="id"></param>
        public void RemoveUser(string userId)
        {
            lock (_obj)
            {
                var index = Users.Select(x => x.UserID).ToList().IndexOf(userId);
                if (index != -1 && Users.Select(x => x.UserID).Contains(userId))
                {
                    Users.RemoveAt(index);
                }
            }
        }
        /// <summary>
        /// 修改用户连接ID
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="connectionId"></param>
        public void UpdateUser(string userId, string connectionId)
        {
            lock (_obj)
            {
                HubClientDTO user = Users.Where(x => x.UserID == userId).FirstOrDefault();
                if (user != null)
                {
                    user.ConnectionId = connectionId;
                }
            }
        }
        /// <summary>
        /// 获取用户连接ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetUserConnectionId(string userId)
        {
            return Users.Where(x => x.UserID == userId).FirstOrDefault()?.ConnectionId;
        }
    }
}
