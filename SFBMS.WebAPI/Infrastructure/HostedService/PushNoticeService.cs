using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFBMS.WebAPI.Infrastructure.HostedService
{
    public class PushNoticeService : BackgroundService
    {
        private readonly ILogger<PushNoticeService> _logger;
        public int Count { get; set; }
        public PushNoticeService(ILogger<PushNoticeService> logger)
        {
            _logger = logger;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await PushNoticeToUserAsync(stoppingToken);
                await Task.Delay(20000, stoppingToken);
            }
        }
        public override Task StopAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("推行通知服务正在关闭");
            return Task.CompletedTask;
        }
        public async Task PushNoticeToUserAsync(CancellationToken stoppingToken)
        {
            try
            {
                //List<MessageInfo> data = await _dbContext.Set<MessageInfo>().Include(x => x.log).AsNoTracking().ToListAsync();               
                Console.WriteLine($"第{Count++}此运行后台服务");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                await Task.Delay(2000, stoppingToken);
            }

        }
    }
}
