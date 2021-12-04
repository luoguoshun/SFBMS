using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SFBMS.Contracts.BookModule;
using SFBMS.Service.BookModule;
using SFBMS.SignalR;
using SFBMS.SignalR.ClientServer;
using SFBMS.SignalR.Hub;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SFBMS.WebAPI.Infrastructure.HostedService
{
    public class StatisticsSubscribeService : BackgroundService
    {
        private IHubContext<ChatHub, IChatHub> _chatHubContext { get; }
        private ISubscribeService _subscribeService { get; }
        private IChatClientServer _chatClientServer { get; }
        public StatisticsSubscribeService(IHubContext<ChatHub, IChatHub> chatHubContext, ISubscribeService subscribeService, IChatClientServer chatClientServer)
        {
            _chatHubContext = chatHubContext;
            _subscribeService = subscribeService;
            _chatClientServer = chatClientServer;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await PushStatisticsAsync("luo");
                await Task.Delay(5000, stoppingToken);
            }
        }
        public override Task StopAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("推行订阅统计正在关闭");
            return Task.CompletedTask;
        }
        /// <summary>
        /// 推送订阅统计
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        private async Task PushStatisticsAsync(string userId)
        {
            var connectionId = _chatClientServer.GetUserConnectionId(userId);
            var data = await _subscribeService.StatisticsAsync();           
            if (connectionId != null && data.Any())
            {
                 Console.WriteLine("\n" + "推送订阅统计:" + JsonConvert.SerializeObject(data) + "\n");
                _chatHubContext.Clients.Client(connectionId).GetStatisticsSubscribe(data);
            }
        }
    }
}
