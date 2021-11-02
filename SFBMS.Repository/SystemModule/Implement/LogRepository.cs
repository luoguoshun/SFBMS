using Dapper;
using Microsoft.Data.SqlClient;
using SFBMS.Contracts.LogModule;
using SFBMS.Entity.Context;
using SFBMS.Entity.LogModule;
using SFBMS.Repository.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SFBMS.Repository.SystemModule.Implement
{
    public class LogRepository : Repository<NLogInfo>, ILogRepository
    {
        public LogRepository(SFBMSContext dbContext) : base(dbContext)
        {
            
        }
        /// <summary>
        /// 获取日志列表
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<LogOutDTO> GetLogListAsync(SelectLogDTO dto)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
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
                if (dto.Dates.Length>0&&dto.Dates!=null)
                {
                    where.Append(" and l.Date BETWEEN @BeginTime AND @EndTime");
                    paramters.Add("BeginTime", dto.Dates[0]);
                    paramters.Add("EndTime", dto.Dates[1]);
                }
                LogOutDTO data = new LogOutDTO
                {
                    Logs = await connection.QueryAsync<LogDTO>($@"
                     select *
                     from NLogInfo as l
                     Where {where}", paramters),
                    Count = await connection.QueryFirstOrDefaultAsync<int>($@"
                     select COUNT(*)
                     from NLogInfo as c              
                     Where {where}", paramters)
                };
                return data;
            }
        }
    }
}
