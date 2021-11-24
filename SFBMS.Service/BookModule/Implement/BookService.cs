using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SFBMS.Common.DocumentHelper;
using SFBMS.Common.EnumList;
using SFBMS.Contracts.BookModule;
using SFBMS.Entity.BookModule;
using SFBMS.Repository.BookModule;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SFBMS.Service.BookModule.Implement
{
    public class BookService : IBookService
    {
        public readonly IBookRepository _bookRepository;
        public readonly IBookTypeRepository _bookTypeRepositoty;
        public readonly ILogger<BookService> _logger;
        public readonly Spreadsheet _spreadsheet;
        public BookService(IBookRepository bookRepository, IBookTypeRepository bookTypeRepositoty, ILogger<BookService> logger, Spreadsheet spreadsheet)
        {
            _bookRepository = bookRepository;
            _bookTypeRepositoty = bookTypeRepositoty;
            _logger = logger;
            _spreadsheet = spreadsheet;
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
            bookIds.ToList().ForEach(async item =>
            {
                var book = await _bookRepository.GetEntityAsync(x => x.Id == item);
                if(!(book is null))
                {
                    _bookRepository.DeleteEntity(book);
                }         
            });
            return await _bookRepository.SaveChangeAsync();
        }
        /// <summary>
        /// 修改书籍部分信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<bool> UpdateSectionBookAsync(UpdateBookDTO dto)
        {
            Book book = await _bookRepository.GetEntityAsync(x => x.Id == dto.Id);
            if (book is null)
            {
                return false;
            }
            book.TypeId = dto.TypeId;
            book.Price = dto.Price;
            book.Inventory = dto.Inventory;
            book.State = dto.State;
            return await _bookRepository.SaveChangeAsync();
        }
        /// <summary>
        /// 更新全部书籍信息
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public async Task<(bool, string)> UpdateAllBookAsync(IFormFile imageFile, UpdateBookDTO dto)
        {
            var book = await _bookRepository.GetEntityAsync(x => x.Id == dto.Id);
            if (book is null)
            {
                return (false, "找不到数据");
            }
            else if (imageFile != null)
            {
                var exist = await SaveImageAsync(imageFile);
                if (!exist.Item1)
                {
                    return (false, "图片保存到服务器失败");
                }
                book.CoverImgSrc = $"/src/Book/images/{exist.Item2}";
            }
            book.BookName = dto.BookName;
            book.TypeId = dto.TypeId;
            book.Price = dto.Price;
            book.Inventory = dto.Inventory;
            book.Descripcion = dto.Descripcion;
            book.PublicationDate = dto.PublicationDate;
            return (await _bookRepository.SaveChangeAsync(), "");
        }
        /// <summary>
        /// 新增书籍
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<bool> CreateBooksAsync(List<CreateBookDTO> dto)
        {
            dto.ForEach(async item =>
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
                    CoverImgSrc = string.Empty,
                    State = 1,
                    CreateTime = DateTime.Now,
                };
                await _bookRepository.AddEntityAsync(book);
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
            //统计导入和修改的数目
            int updateCount=0, ImportCount=0;
            (List<ImportBookDTO>, string) data = _spreadsheet.GetDataListFromExcel<ImportBookDTO>(stream);
            if (!data.Item1.Any())
            {
                foreach (var item in data.Item1)
                {
                    Book bookData = await _bookRepository.GetEntityAsync(x => x.BookName.Contains(item.BookName));
                    if (bookData != null)
                    {
                        bookData.Inventory += item.Inventory;
                        updateCount++;
                    }
                    else
                    {
                        //获取到书籍类型字符串转成对应的BookTypes枚举类型
                        int typeId = 0;
                        string[] enumNames = typeof(BookTypes).GetEnumNames();
                        if (enumNames.Contains(item.TypeName))
                        {
                            //Parse(enumType,value)将枚举常量转换为等效的枚举对象
                            typeId = (int)Enum.Parse(typeof(BookTypes), item.TypeName);
                        }
                        Book book = new Book
                        {
                            BookName =item.BookName,
                            TypeId = typeId,
                            Author = item.Author,
                            Press = item.Press,
                            PublicationDate = item.PublicationDate,
                            Price = item.Price,
                            Inventory = item.Inventory,
                            Descripcion = item.Descripcion,
                            CoverImgSrc = string.Empty,
                            CreateTime = DateTime.Now,
                        };
                        await _bookRepository.AddEntityAsync(book);
                        ImportCount++;
                    }
                }
                return (await _bookRepository.SaveChangeAsync(), $"成功导入{ImportCount}条、修改{updateCount}条数据");
            }
            return (false, data.Item2);
        }
        /// <summary>
        /// 保存图片到服务器
        /// </summary>
        /// <param name="imageFile"></param>
        /// <returns></returns>
        public async Task<(bool, string)> SaveImageAsync(IFormFile imageFile)
        {
            //string fileName = Guid.NewGuid().ToString();
            //string extName = Path.GetExtension(imageFile.FileName).ToLower(); /*获取文件名后缀*/
            string basePath = Environment.CurrentDirectory + @"\wwwroot\Book\images\";
            string ImageUrl = basePath + imageFile.FileName;
            if (!Directory.Exists(basePath))
            {
                Directory.CreateDirectory(basePath);
            }
            //打开文件并保存到指定文件夹中
            using (var stream = new FileStream(ImageUrl, FileMode.OpenOrCreate))
            {
                await imageFile.CopyToAsync(stream);
            }
            return (File.Exists(ImageUrl), imageFile.FileName);
        }
    }
}
