using Microsoft.Extensions.Configuration;
using System.IO;
using System.Linq;

namespace SFBMS.Common.SiteConfig
{
    /// <summary>
    /// 配置信息读取模型
    /// </summary>
    public class SiteConfigHelper
    {
        public IConfiguration _configuration { get; }
        public SiteConfigHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string GetSectioValue1(string[] sections)
        {
            IConfigurationSection configSection = null;
            for (int i = 0; i < sections.Length; i++)
            {
                if (i == 0)
                {
                    configSection = _configuration.GetSection(sections[i]);
                }
                else
                {
                    configSection = configSection.GetSection(sections[i]);
                }
            }
            return configSection.Value;
        }
        public string GetSectioValue(string[] sections)
        {
            //添加 json 配置文件路径
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            //创建配置根对象
            IConfigurationRoot configurationRoot = builder.Build();
            IConfigurationSection configSection = null;
            for (int i = 0; i < sections.Length; i++)
            {
                if (i == 0)
                {
                    configSection = configurationRoot.GetSection(sections[i]);
                }
                else
                {
                    configSection = configSection.GetSection(sections[i]);
                }            
            }
            return configSection.Value;
        }
    }
}
