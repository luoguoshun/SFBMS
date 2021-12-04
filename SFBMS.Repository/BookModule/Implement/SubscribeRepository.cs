using Dapper;
using Microsoft.Data.SqlClient;
using SFBMS.Contracts.BookModule;
using SFBMS.Entity.BookModule;
using SFBMS.Entity.Context;
using SFBMS.Repository.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFBMS.Repository.BookModule.Implement
{
    public class SubscribeRepository : Repository<Subscribe>, ISubscribeRepository
    {
        public SubscribeRepository(SFBMSContext dbContext) : base(dbContext)
        {
        }
        /// <summary>
        /// 获取订阅书籍列表
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<SubscribeDTO>> GetSubscribeListAsync()
        {
            using var connection = new SqlConnection(ConnectionString);
            var data = await connection.QueryAsync<SubscribeDTO>(@"
                           select bu.BookId,bu.ClientNo,bu.CreateTime,
                           cli.[Name] as [ClientName], bk.BookName, bt.TypeName
                           from Subscribe as bu
                           left join ClientInfo as cli on bu.ClientNo = cli.ClientNo
                           left join BookInfo as bk on bu.BookId = bk.Id
                           left join BookType as bt on bk.TypeId = bt.TypeId");
            return data;
        }
        /// <summary>
        /// 统计订阅信息
        /// </summary>
        /// <returns></returns>
        public async Task<IList<StatisticsSubscribeDTO>> StatisticsAsync()
        {
            using var connection = new SqlConnection(ConnectionString);
            var data = await connection.QueryAsync<StatisticsSubscribeDTO>(@"
                           select count(*) as [value],bt.TypeName as [name]
                           from Subscribe as bu
                           left join BookInfo as bk on bu.BookId = bk.Id
                           left join BookType as bt on bk.TypeId = bt.TypeId
                           group by bt.TypeName
                           order by value desc");
            return (IList<StatisticsSubscribeDTO>)data;
        }
    }
}
