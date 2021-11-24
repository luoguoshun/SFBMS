using SFBMS.Contracts.BookModule;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace SFBMS.Service.BookModule
{
    public interface IBookService
    {
        Task<BookOutDTO> GetBookListAsync(SelectBookDTO dto);
        Task<IList<BookTypeDTO>> GetBookTypeListAsync();
        Task<bool> DeleteBooksAsync(int[] bookIds);
        Task<(bool, string)> UpdateAllBookAsync(IFormFile imageFile, UpdateBookDTO dto);
        Task<bool> UpdateSectionBookAsync(UpdateBookDTO dto);
        Task<bool> CreateBooksAsync(List<CreateBookDTO> dto);
        Task<(bool, string)> ImportBooksAsync(Stream stream); 
        Task<(bool, string)> SaveImageAsync(IFormFile imageFile);
    }
}
