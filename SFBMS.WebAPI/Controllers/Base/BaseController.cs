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
        protected virtual IActionResult JsonResult(bool success, string message)
        {
            return Ok(new MyApiResult
            {
                Success = success,
                Message = message,
                Data = null
            });
        }
        protected virtual IActionResult JsonSuccess(string message, object data)
        {
            return Ok(new MyApiResult
            {
                Success = true,
                Message = message,
                Data = data
            });
        }
        protected virtual IActionResult JsonSuccess(string message)
        {
            return Ok(new MyApiResult
            {
                Success = true,
                Message = message,
            });
        }
        protected virtual IActionResult JsonSuccess(object data)
        {
            return Ok(new MyApiResult
            {
                Success = true,
                Data = data,
            });
        }
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
