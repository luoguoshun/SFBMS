using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using SFBMS.Contracts.SystemModule;
using SFBMS.Service.SystemModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SFBMS.WebAPI.Infrastructure.ExceptionsFilter
{
    /// <summary>
    /// 全局异常处理过滤器(只能管到Controller而已 之外的异常无法捕获)
    /// </summary>
    public class GlobalExceptionsFilter : IAsyncExceptionFilter
    {
        private readonly ILogger<GlobalExceptionsFilter> _logger;
        private ILogService _logService;
        public GlobalExceptionsFilter(ILogger<GlobalExceptionsFilter> logger, ILogService logService)
        {
            _logger = logger;
            _logService = logService;
        }
        public Task OnExceptionAsync(ExceptionContext context)
        {
            if (context.ExceptionHandled == false)
            {
                Exception ex = context.Exception;
                string ip = context.HttpContext.Connection.RemoteIpAddress.ToString();
                string routeInfo = context.HttpContext.Request.Path.Value;
                string errorString = $"全局异常{{主机IP:{ip};来源:{ex.Source};异常接口:{routeInfo}; 异常描述:{ex.Message}}}";
                context.Result = new JsonResult("Controller全局异常拦截：" + errorString);
                _logger.LogError(errorString);
                RecordNLog(ip, routeInfo, ex.Source, ex.Message);
            }
            context.ExceptionHandled = true; //异常已处理
            return Task.CompletedTask;
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
