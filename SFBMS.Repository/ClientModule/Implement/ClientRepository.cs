using Dapper;
using Microsoft.Data.SqlClient;
using SFBMS.Contracts.ClientModule;
using SFBMS.Entity.ClientModule;
using SFBMS.Entity.Context;
using SFBMS.Repository.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SFBMS.Repository.ClientModule.Implement
{
    public class ClientRepository : Repository<Client>, IClientRepository
    {
        public ClientRepository(SFBMSContext dbContext) : base(dbContext)
        {
        }
        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<ClientOutDTO> GetClientListAsync(SelectClientDTO dto)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                DynamicParameters paramters = new DynamicParameters();
                StringBuilder where = new StringBuilder("1=1");
                if(!string.IsNullOrWhiteSpace(dto.Name))
                {
                    where.Append(" and c.Name like @Name ");
                    paramters.Add("Name", $@"%{dto.Name}%");
                }
                if (!string.IsNullOrWhiteSpace(dto.Phone))
                {
                    where.Append(" and c.Phone like @Phone ");
                    paramters.Add("Phone", $@"%{dto.Phone}%");
                }
                ClientOutDTO data = new ClientOutDTO
                {
                    Clients= await connection.QueryAsync<ClientDTO>($@"
                     select c.ClientNo,c.Name,c.Sex,c.IdNumber,c.BirthDate,c.Address,c.Phone,c.Flag
                     from ClientInfo as c 
                     Where {where}", paramters),
                    Count = await connection.QueryFirstOrDefaultAsync<int>($@"
                     select COUNT(*)
                     from ClientInfo as c              
                     Where {where}", paramters)
                };
                return data;
            }
        }
    }
}
