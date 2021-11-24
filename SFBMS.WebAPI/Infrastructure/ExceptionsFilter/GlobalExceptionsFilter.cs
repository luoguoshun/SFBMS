using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
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
        public GlobalExceptionsFilter(ILogger<GlobalExceptionsFilter> logger)
        {
            _logger = logger;
        }
        public Task OnExceptionAsync(ExceptionContext context)
        {
            if (context.ExceptionHandled == false)
            {
                Exception ex = context.Exception;/*这里给系统分配标识，监控异常肯定不止一个系统。*/
                int sysId = 1;/*监控了ip方便定位到底是那台服务器出故障了*/
                string ip = context.HttpContext.Connection.RemoteIpAddress.ToString();
                string uri = context.HttpContext.Request.Path.Value;
                string errorString = $"全局异常{{系统编号:{sysId};主机IP:{ip};异常接口:{uri}; 异常描述:{ex.Message}}}";
                _logger.LogError(errorString);
                context.Result = new JsonResult("Controller全局异常拦截：" + ex.Message);
            }
            context.ExceptionHandled = true; //异常已处理了
            return Task.CompletedTask;
        }
    }
}
