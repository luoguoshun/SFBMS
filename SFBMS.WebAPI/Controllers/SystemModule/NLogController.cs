using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OHEXML.Infrastructure.Attributes;
using SFBMS.Common.Algorithm;
using SFBMS.Common.EnumList;
using SFBMS.Contracts.SystemModule;
using SFBMS.Service.SystemModule;
using SFBMS.WebAPI.Controllers.Base;

namespace SFBMS.WebAPI.Controllers.SystemModule
{
    public class NLogController : BaseController
    {
        #region 构造函数
        public ILogService _logService;
        public NLogController(ILogService logService)
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
        public async Task<IActionResult> GetNLogList(SelectNLogDTO dto)
        {
            var logs = await _logService.GetNLogListAsync(dto);
            return JsonSuccess(logs);
        }
    }
}
