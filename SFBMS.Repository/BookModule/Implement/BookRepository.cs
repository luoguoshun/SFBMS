using Dapper;
using Microsoft.Data.SqlClient;
using SFBMS.Contracts.BookModule;
using SFBMS.Entity.BookModule;
using SFBMS.Entity.Context;
using SFBMS.Repository.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFBMS.Repository.BookModule.Implement
{
    public class BookRepository : Repository<Book>, IBookRepository
    {
        public BookRepository(SFBMSContext dbContext) : base(dbContext)
        {

        }
        /// <summary>
        /// 获取书籍列表
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<BookOutDTO> GetBookListAsync(SelectBookDTO dto)
        {
            using var connection = new SqlConnection(ConnectionString);
            DynamicParameters paramters = new DynamicParameters();
            StringBuilder where = new StringBuilder("1=1");
            if (!string.IsNullOrWhiteSpace(dto.Conditions))
            {
                where.Append(" and b.BookName like @BookName or b.Author like @Author or b.Descripcion like @Descripcion ");
                paramters.Add("BookName", $@"%{dto.Conditions}%");
                paramters.Add("Author", $@"%{dto.Conditions}%");
                paramters.Add("Descripcion", $@"%{dto.Conditions}%");
            }
            if (!(dto.TypeId is 0))
            {
                where.Append(" and b.TypeId = @TypeId ");
                paramters.Add("TypeId", dto.TypeId);
            }
            if (!(dto.PublicationDates is null) && dto.PublicationDates.Length > 0)
            {
                where.Append(" and b.PublicationDate BETWEEN @BeginTime AND @EndTime");
                paramters.Add("BeginTime", dto.PublicationDates[0]);
                paramters.Add("EndTime", dto.PublicationDates[1]);
            }
            paramters.Add("row", dto.Row);
            paramters.Add("page", dto.Page);
            BookOutDTO result = new BookOutDTO
            {
                Books = await connection.QueryAsync<BookDTO>($@"
                     select b.Id,b.BookName,b.Author,b.Press,b.PublicationDate,b.Price,b.Inventory,b.CoverImgSrc,b.Descripcion,b.State,
                     t.TypeId,t.TypeName
                     from BookInfo as b
                     left join BookType as t on b.TypeId=t.TypeId
                     Where {where}
                     order by b.Id asc
                     offset @row * (@page - 1) rows fetch next @row rows only", paramters),
                Count = await connection.QueryFirstOrDefaultAsync<int>($@"
                     select COUNT(*)
                     from BookInfo as b 
                     left join BookType as t on b.TypeId=t.TypeId
                     Where {where}", paramters)
            };
            return result;
        }
    }
}
