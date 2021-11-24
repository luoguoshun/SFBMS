using Microsoft.Extensions.Configuration;
using System.IO;
using System.Linq;
using System;

namespace SFBMS.Common.SiteConfig
{
    public static class SiteConfigHelper
    {
        /// <summary>
        /// 读取配置文件信息
        /// </summary>
        /// <param name="sections">结点集合 按照配置文件结构存放数组</param>
        /// <returns></returns>
        public static string GetSectionValue(string section)
        {
            //添加 json 配置文件路径
            var builder = new ConfigurationBuilder()
                          .SetBasePath(Directory.GetCurrentDirectory())
                          .AddJsonFile("appsettings.json");
            //创建配置根对象
            IConfigurationRoot configurationRoot = builder.Build();
            return configurationRoot.GetSection(section).Value;
        }
    }
}
