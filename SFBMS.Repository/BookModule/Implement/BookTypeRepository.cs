using SFBMS.Entity.BookModule;
using SFBMS.Entity.Context;
using SFBMS.Repository.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace SFBMS.Repository.BookModule.Implement
{
    public class BookTypeRepository : Repository<BookType>, IBookTypeRepository
    {
        public BookTypeRepository(SFBMSContext dbContext):base(dbContext)
        {
        }
    }
}
