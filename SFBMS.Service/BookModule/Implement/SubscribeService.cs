using SFBMS.Contracts.BookModule;
using SFBMS.Repository.BookModule;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SFBMS.Service.BookModule.Implement
{
    public class SubscribeService : ISubscribeService
    {
        public ISubscribeRepository _subscribeRepositpry;
        public SubscribeService(ISubscribeRepository subscribeRepositpry)
        {
            _subscribeRepositpry = subscribeRepositpry;
        }
        /// <summary>
        /// 获取订阅列表
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<SubscribeDTO>> GetSubscribeListAsync()
        {
            return await _subscribeRepositpry.GetSubscribeListAsync();
        }
        //统计订阅排行
        public async Task<IList<StatisticsSubscribeDTO>> StatisticsAsync()
        {
            return await _subscribeRepositpry.StatisticsAsync();
        }
    }
}
