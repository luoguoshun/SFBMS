using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OHEXML.Infrastructure.Attributes;
using SFBMS.Common.EnumList;
using SFBMS.Service.SystemModule;
using SFBMS.WebAPI.Controllers.Base;

namespace SFBMS.WebAPI.Controllers.SystemModule
{
    public class LogController : BaseController
    {
        #region 构造函数
        public ILogService _logService;
        public LogController(ILogService logService)
        {
            _logService = logService;
        }
        #endregion

        /// <summary>
        /// 获取日志列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AppAuthorize(AppTypes.Background)]
        public async Task<IActionResult> tt()
        {
            var logs = await _logService.GetLogList();
            if (logs is null)
            {
                return JsonFailt("");
            }
            return JsonSuccess("", logs);
        }
    }
}
