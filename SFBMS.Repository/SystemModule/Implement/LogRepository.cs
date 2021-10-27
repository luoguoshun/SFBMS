using SFBMS.Entity.Context;
using SFBMS.Entity.LogModule;
using SFBMS.Repository.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace SFBMS.Repository.SystemModule.Implement
{
    public class LogRepository : Repository<NLogInfo>, ILogRepository
    {
        public LogRepository(SFBMSContext dbContext) : base(dbContext)
        {
            
        }
    }
}
