using Microsoft.EntityFrameworkCore;
using SFBMS.Common.Extentions;
using SFBMS.Entity.Base;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace SFBMS.Entity.Context
{
   public class SFBMSContext:DbContext
    {
        /// <summary>
        /// 这是将 AddDbContext 的上下文配置传递到 DbContext 的方式
        /// </summary>
        /// <param name="options"></param>
        public SFBMSContext(DbContextOptions<SFBMSContext> options) : base(options)
        {
        }

        /// <summary>
        /// 创建模型
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
              .LoadEntityConfiguration<SFBMSContext>()
              .AddEntityTypes<BaseEntity>();
            //在程序集中(SFBMS.Entity)实现 IEntityTypeConfiguration 的类型中指定的所有配置
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.Load("SFBMS.Entity"));
        }
    }
}
