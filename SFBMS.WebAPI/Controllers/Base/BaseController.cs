using Microsoft.AspNetCore.Mvc;
using System;
using SFBMS.WebAPI.Infrastructure.Models;
using SFBMS.Common.Context;
using SFBMS.Common.Extentions;

namespace SFBMS.WebAPI.Controllers.Base
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public abstract class BaseController : ControllerBase
    {
        private ApiContext _apiContext;

        /// <summary>
        /// 获取Api上下文
        /// </summary>
        protected virtual ApiContext ApiContext()
        {
            if (_apiContext == null)
            {
                try
                {
                    _apiContext = new ApiContext
                    {
                        User = new ApiUser
                        {
                            Id = HttpContext.User.GetClaimValue<string>("Id"),
                            Name = HttpContext.User.GetClaimValue<string>("Name"),
                        }
                    };
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return _apiContext;
        }
        protected virtual IActionResult JsonResult<T>(bool success, string message, T data)
        {
            return Ok(new MyApiResult
            {
                Success = success,
                Message = message,
                Data = data
            });
        }
        /// <summary>
        /// 定义成功的返回值
        /// </summary>
        /// <param name="message"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        protected virtual IActionResult JsonSuccess(string message, object data)
        {
            return Ok(new MyApiResult
            {
                Success = true,
                Message = message,
                Data = data
            });
        }
        /// <summary>
        /// 定义失败的返回值
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        protected virtual IActionResult JsonFailt(string message)
        {
            return Ok(new MyApiResult
            {
                Success = false,
                Message = message
            });
        }
    }
}
