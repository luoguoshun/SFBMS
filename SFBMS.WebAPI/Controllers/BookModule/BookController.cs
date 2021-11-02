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
            var books = await _bookServe.GetBookListAsync(dto);
            if (books is null)
            {
                return JsonFailt("");
            }
            return JsonSuccess("", books);
        }
        /// <summary>
        /// 获取书籍类型列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AppAuthorize(AppTypes.Background)]
        public async Task<IActionResult> GetBookTypeList()
        {
            var types = await _bookServe.GetBookTypeListAsync();
            if (types is null)
            {
                return JsonFailt("无数据");
            }
            return JsonSuccess("", types);
        }
        /// <summary>
        /// 删除书籍
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [AppAuthorize(AppTypes.Background)]
        public async Task<IActionResult> DeleteBooks(DeleteBookDTO dto)
        {
            var types = await _bookServe.DeleteBooksAsync(dto.BookIds);
            if (!types)
            {
                return JsonFailt("删除失败");
            }
            return JsonSuccess("删除成功", types);
        }
        [HttpPost]
        [AppAuthorize(AppTypes.Background)]
        public async Task<IActionResult> UpdateBook(UpdateBookDTO dto)
        {
            bool types = await _bookServe.UpdateBooksAsync(dto);
            if (!types)
            {
                return JsonFailt("修改失败");
            }
            return JsonSuccess("删修改功", types);
        }
    }
}
