using Microsoft.Extensions.Logging;
using SFBMS.Common.DocumentHelper;
using SFBMS.Contracts.BookModule;
using SFBMS.Entity.BookModule;
using SFBMS.Repository.BookModule;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFBMS.Service.BookModule.Implement
{
    public class BookService : IBookService
    {
        public readonly IBookRepository _bookRepository;
        public readonly IBookTypeRepository _bookTypeRepositoty;
        public readonly ILogger _logger;
        public readonly Spreadsheet _spreadsheet;
        public BookService(IBookRepository bookRepository, IBookTypeRepository bookTypeRepositoty, ILogger logger)
        {
            _bookRepository = bookRepository;
            _bookTypeRepositoty = bookTypeRepositoty;
            _logger = logger;
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
            var data = await _bookTypeRepositoty.GetAllAsync();
            return data.Select(types => new BookTypeDTO { TypeId = types.TypeId, TypeName = types.TypeName }).ToList();
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
            if (book is null)
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
        /// <summary>
        /// 新增书籍
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<bool> CreateBooksAsync(List<CreateBookDTO> dto)
        {
            dto.ForEach(item =>
            {
                Book book = new Book
                {
                    BookName = item.BookName,
                    TypeId = item.TypeId,
                    Author = item.Author,
                    Press = item.Press,
                    PublicationDate = item.PublicationDate,
                    Price = item.Price,
                    Inventory = item.Inventory,
                    Descripcion = item.Descripcion,
                    ImageSrc = string.Empty,
                    CreateTime = DateTime.Now,
                };
                _bookRepository.AddEntityAsync(book);
            });
            return await _bookRepository.SaveChangeAsync();
        }
        /// <summary>
        /// 通过Excel导入书籍
        /// </summary>
        /// <param name="stream">文件流</param>
        /// <returns></returns>
        public async Task<(bool, string)> ImportBooksAsync(Stream stream)
        {
            (List<Book>, string) data = _spreadsheet.GetDataListFromExcel<Book>(stream);
            if (!(data.Item1 is null))
            {
                await _bookRepository.AddEntitiesAsync(data.Item1);
                return (await _bookRepository.SaveChangeAsync(), "导入成功");
            }
            return (false, data.Item2);
        }
    }
}
