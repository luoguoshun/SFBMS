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
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Reflection;

namespace SFBMS.WebAPI.Controllers.BookModule
{
    public class BookController : BaseController
    {
        public readonly IBookService _bookServe;
        public readonly ILogger<BookController> _logger;
        public BookController(IBookService bookServe, ILogger<BookController> logger)
        {
            _bookServe = bookServe;
            _logger = logger;
        }
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
            return JsonSuccess(books);
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
            return JsonSuccess(types);
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
            return JsonSuccess("删除成功");
        }
        /// <summary>
        /// 修改部分书籍信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [AppAuthorize(AppTypes.Background)]
        public async Task<IActionResult> UpdateSectionBook(UpdateBookDTO dto)
        {
            bool result = await _bookServe.UpdateSectionBookAsync(dto);
            if (!result)
            {
                return JsonFailt("修改失败");
            }
            return JsonSuccess("删修改功");
        }
        /// <summary>
        /// 修改全部书籍信息
        /// </summary>
        /// <param name="file"></param>
        /// <param name="bookId">s书籍编号</param>
        /// <returns></returns>
        [HttpPost]
        [AppAuthorize(AppTypes.Background)]
        public async Task<IActionResult> UpdateAllBook()
        {
            //接受图片
            IFormFile imageFile = Request.Form.Files.FirstOrDefault();
            UpdateBookDTO dto = new UpdateBookDTO();
            PropertyInfo[] properties = dto.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            properties.ToList().ForEach(item =>
            {
                string proName = item.Name[0].ToString().ToLower() + item.Name.Substring(1); /*首字母小写*/
                //获取前端传来Formdata里的值
                var data = Request.Form[proName];
                object value = Convert.ChangeType(data.ToString(), item.PropertyType);
                item.SetValue(dto, value, null);
            });
            var result = await _bookServe.UpdateAllBookAsync(imageFile, dto);
            if (result.Item1)
            {
                return JsonSuccess("删修改功");
            }
            return JsonFailt(result.Item2);
        }
        /// <summary>
        /// 新增书籍
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [AppAuthorize(AppTypes.Background)]
        public async Task<IActionResult> CreatesBook(List<CreateBookDTO> dto)
        {
            bool result = await _bookServe.CreateBooksAsync(dto);
            if (!result)
            {
                return JsonFailt("添加失败");
            }
            return JsonSuccess("添加成功");
        }
        /// <summary>
        /// 通过Excel导入书籍
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        [AppAuthorize(AppTypes.Background)]
        public async Task<IActionResult> ImportBooks(IFormFile file)
        {
            var result = await _bookServe.ImportBooksAsync(file.OpenReadStream());
            return JsonResult(result.Item1, result.Item2);
        }
    }
}
