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
using System.Linq;

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
                if(!string.IsNullOrWhiteSpace(dto.Conditions))
                {
                    where.Append(" and c.Name like @Name or c.Phone like @Phone ");
                    paramters.Add("Name", $@"%{dto.Conditions}%");
                    paramters.Add("Phone", $@"%{dto.Conditions}%");
                }
                if (dto.RoleId!=0)
                {
                    where.Append(" and r.RoleId = @RoleId ");
                    paramters.Add("RoleId", dto.RoleId);
                }
                paramters.Add("row", dto.Row);
                paramters.Add("page", dto.Page);
                ClientOutDTO data = new ClientOutDTO
                {
                    Clients= await connection.QueryAsync<ClientDTO>($@"
                     select c.ClientNo,c.Name,c.Sex,c.IdNumber,c.BirthDate,c.Address,c.Phone,c.State,c.CreateTime,c.HeaderImgSrc,
                     STRING_AGG(r.Name, '、') as RoleName
                     from ClientInfo as c 
                     left join User_Role as ur on c.ClientNo = ur.UserId
                     left join RoleInfo as r on r.RoleId = ur.RoleId                     
                     where {where}
                     GROUP BY c.ClientNo,c.Name,c.Sex,c.IdNumber,c.BirthDate,c.Address,c.Phone,c.State,c.CreateTime,HeaderImgSrc
                     order by c.CreateTime asc
                     offset @row * (@page - 1) rows fetch next @row rows only", paramters),
                    Count = await connection.QueryFirstOrDefaultAsync<int>($@"
                     select COUNT(*)
                     from ClientInfo as c
                     left join User_Role as ur on c.ClientNo = ur.UserId
                     left join RoleInfo as r on r.RoleId = ur.RoleId
                     where {where}
                     GROUP BY c.ClientNo,c.Name,c.Sex,c.IdNumber,c.BirthDate,c.Address,c.Phone,c.State,c.CreateTime", paramters)

                };
                return data;
            }
        }
    }
}
