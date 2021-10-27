using System;
using NLog;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OHEXML.Infrastructure.Attributes;
using SFBMS.Common.EnumList;
using SFBMS.Contracts.BookModule;
using SFBMS.Service.BookModule;
using SFBMS.WebAPI.Controllers.Base;
using Microsoft.Extensions.Logging;

namespace SFBMS.WebAPI.Controllers.BookModule
{
    public class BookController : BaseController
    {
        #region 构造函数
        public readonly IBookService _bookServe;

        public readonly ILogger<BookController> _logger;
        public BookController(IBookService bookServe, ILogger<BookController> logger)
        {
            _bookServe = bookServe;
            _logger = logger;
        }
        #endregion

        /// <summary>
        /// 获取书籍列表
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [AppAuthorize(AppTypes.Background)]
        public async Task<IActionResult> GetBookList(SelectBookDTO dto)
        {
            _logger.LogError("ErrorTest");
            var data = await _bookServe.GetBookListAsync(dto);
            return data != null ? JsonSuccess("OK", data) : JsonFailt("NO");
        }
        /// <summary>
        /// 获取书籍类型列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AppAuthorize(AppTypes.Background)]
        public async Task<IActionResult> GetBookTypeList()
        {
            var data = await _bookServe.GetBookTypeListAsync();
            return data != null ? JsonSuccess("OK", data) : JsonFailt("NO");
        }
    }
}
