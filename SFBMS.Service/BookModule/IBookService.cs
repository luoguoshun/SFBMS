using SFBMS.Contracts.BookModule;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SFBMS.Service.BookModule
{
    public interface IBookService
    {
        /// <summary>
        /// 获取书籍列表
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<BookOutDTO> GetBookListAsync(SelectBookDTO dto);
        /// <summary>
        /// 获取书籍类型列表
        /// </summary>
        /// <returns></returns>
        Task<IList<BookTypeDTO>> GetBookTypeListAsync();
    }
}
