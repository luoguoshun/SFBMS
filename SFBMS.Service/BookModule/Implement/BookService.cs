using SFBMS.Contracts.BookModule;
using SFBMS.Repository.BookModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFBMS.Service.BookModule.Implement
{
    public class BookService : IBookService
    {
        public readonly IBookRepository _bookRepository;
        public readonly IBookTypeRepository _bookTypeRepositoty;
        public BookService(IBookRepository bookRepository, IBookTypeRepository bookTypeRepositoty)
        {
            _bookRepository = bookRepository;
            _bookTypeRepositoty = bookTypeRepositoty;
        }
       
        /// <summary>
        /// 获取书籍列表
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<BookOutDTO> GetBookListAsync(SelectBookDTO dto)
        {
            return await _bookRepository.GetBookListAsync(dto);
        }
        /// <summary>
        /// 获取书籍类型列表
        /// </summary>
        /// <returns></returns>
        public async Task<IList<BookTypeDTO>> GetBookTypeListAsync()
        {
            var data= await _bookTypeRepositoty.GetAllAsync();
            return data.Select(types => new BookTypeDTO {TypeId = types.TypeId, TypeName = types.TypeName }).ToList();
        }
        /// <summary>
        /// 删除书籍
        /// </summary>
        /// <param name="booksId"></param>
        /// <returns></returns>
        public async Task<bool> DeleteBooksAsync(int[] bookIds)
        {
            foreach (var item in bookIds)
            {
                var book = await _bookRepository.GetEntityAsync(x => x.Id == item);
                _bookRepository.DeleteEntity(book);
            }
            return await _bookRepository.SaveChangeAsync();
        }
        /// <summary>
        /// 修改书籍
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<bool> UpdateBooksAsync(UpdateBookDTO dto)
        {
            var book = await _bookRepository.GetEntityAsync(x => x.Id == dto.Id);
            if(book is null)
            {
                return false;
            }
            book.BookName = dto.BookName;
            book.TypeId = dto.TypeId;
            book.Price = dto.Price;
            book.Inventory = dto.Inventory;
            book.Descripcion = dto.Descripcion;
            book.PublicationDate = dto.PublicationDate;
            return await _bookRepository.SaveChangeAsync();
        }
    }
}
