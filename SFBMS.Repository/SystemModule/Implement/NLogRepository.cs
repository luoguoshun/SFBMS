using Dapper;
using Microsoft.Data.SqlClient;
using SFBMS.Contracts.SystemModule;
using SFBMS.Entity.Context;
using SFBMS.Entity.SystemModule;
using SFBMS.Repository.Base;
using System.Text;
using System.Threading.Tasks;

namespace SFBMS.Repository.SystemModule.Implement
{
    public class NLogRepository : Repository<NLogInfo>, INLogRepository
    {
        public NLogRepository(SFBMSContext dbContext) : base(dbContext)
        {
        }
        /// <summary>
        /// 获取日志列表
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<NLogOutDTO> GetLogListAsync(SelectNLogDTO dto)
        {
            using var connection = new SqlConnection(ConnectionString);
            DynamicParameters paramters = new DynamicParameters();
            StringBuilder where = new StringBuilder("1=1");
            if (!string.IsNullOrWhiteSpace(dto.Level))
            {
                where.Append(" and l.Level like @Level ");
                paramters.Add("Level", $@"%{dto.Level}%");
            }
            if (!string.IsNullOrWhiteSpace(dto.Message))
            {
                where.Append(" and l.Message like @Message ");
                paramters.Add("Message", $@"%{dto.Message}%");
            }
            if (dto.Dates.Length > 0 && dto.Dates != null)
            {
                where.Append(" and l.Date BETWEEN @BeginTime AND @EndTime");
                paramters.Add("BeginTime", dto.Dates[0]);
                paramters.Add("EndTime", dto.Dates[1]);
            }
            paramters.Add("row", dto.Row);
            paramters.Add("page", dto.Page);
            NLogOutDTO data = new NLogOutDTO
            {
                Logs = await connection.QueryAsync<NLogDTO>($@"
                     select l.[LogId],l.[MachineId],l.[Origin],l.[RouteInfo],l.[Level],l.[Message],l.[Detail],l.[Date]
                     from NLogInfo as l
                     Where {where}
                     order by l.Date asc
                     offset @row * (@page - 1) rows fetch next @row rows only", paramters),
                Count = await connection.QueryFirstOrDefaultAsync<int>($@"
                     select COUNT(*)
                     from NLogInfo as l              
                     Where {where}", paramters)
            };
            return data;
        }
    }
}
