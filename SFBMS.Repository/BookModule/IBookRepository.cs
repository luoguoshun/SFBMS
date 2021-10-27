using SFBMS.Contracts.BookModule;
using SFBMS.Entity.BookModule;
using SFBMS.Repository.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SFBMS.Repository.BookModule
{
    public interface IBookRepository : IRepository<Book>
    {
        /// <summary>
        /// 获取书籍列表
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public Task<BookOutDTO> GetBookListAsync(SelectBookDTO dto);
    }
}
