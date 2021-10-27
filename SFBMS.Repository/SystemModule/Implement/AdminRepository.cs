using Microsoft.EntityFrameworkCore.Storage;
using SFBMS.Entity.Context;
using SFBMS.Entity.SystemModule;
using SFBMS.Repository.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace SFBMS.Repository.SystemModule.Implement
{
    public class AdminRepository : Repository<AdminInfo>, IAdminRepository
    {
        public AdminRepository(SFBMSContext dbContext) : base(dbContext)
        {
        }
    }
}
