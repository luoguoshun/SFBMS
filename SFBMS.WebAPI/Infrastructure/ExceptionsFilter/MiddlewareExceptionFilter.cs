using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SFBMS.Contracts.SystemModule;
using SFBMS.Service.SystemModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SFBMS.WebAPI.Infrastructure.ExceptionsFilter
{
    /// <summary>
    /// 全局异常拦截器 GlobalExceptionsFilter无法拦截的在此拦截
    /// </summary>
    public class MiddlewareExceptionFilter
    {
        #region 构造函数
        private readonly RequestDelegate _next;
        private readonly ILogger<MiddlewareExceptionFilter> _logger;
        private ILogService _logService;

        public MiddlewareExceptionFilter(RequestDelegate next, ILogger<MiddlewareExceptionFilter> logger, ILogService logService)
        {
            _next = next;
            _logger = logger;
            _logService = logService;
        }
        #endregion

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }
        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {         
            string ip = context.Connection.RemoteIpAddress.ToString();
            string routeInfo = context.Request.Path.Value;
            string errorString = $"中间件抓捕异常{{主机IP:{ip};来源:{ex.Source};异常接口:{routeInfo}; 异常描述:{ex.Message}}}";          
            context.Response.ContentType = "application/json;charset=utf-8";
            await context.Response.WriteAsync("Middleware全局异常拦截：" + errorString);
            _logger.LogError(errorString);
            RecordNLog(ip, routeInfo, ex.Source, ex.Message);
        }
        /// <summary>
        /// 记录日志到数据库
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public Task RecordNLog(string ip, string routeInfo, string source, string message)
        {
            CreateNLogDTO dto = new CreateNLogDTO
            {
                MachineId = ip,
                RouteInfo = routeInfo,
                Origin = source,
                Level = "error",
                Message = message,
                Detail = "",
            };
            return _logService.CreateNLogAsync(dto);
        }
    }
}
