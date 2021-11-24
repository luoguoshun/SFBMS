using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
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
        public MiddlewareExceptionFilter(RequestDelegate next, ILogger<MiddlewareExceptionFilter> logger)
        {
            _next = next;
            _logger = logger;
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
            int sysId = 1;
            string ip = context.Connection.RemoteIpAddress.ToString();
            var uri = context.Request.Path.Value;
            string errorString = $"中间件抓捕异常{{系统编号:{sysId};主机IP:{ip};异常接口:{uri}; 异常描述:{ex.Message}}}";
            _logger.LogError(errorString);
            context.Response.ContentType = "application/json;charset=utf-8";
            await context.Response.WriteAsync("Middleware全局异常拦截：" + ex.Message);
        }
    }
}
