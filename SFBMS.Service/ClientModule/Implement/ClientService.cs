using Microsoft.AspNetCore.Http;
using SFBMS.Common.Algorithm;
using SFBMS.Common.EnumList;
using SFBMS.Contracts.ClientModule;
using SFBMS.Entity.ClientModule;
using SFBMS.Entity.SystemModule;
using SFBMS.Repository.ClientModule;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SFBMS.Service.ClientModule.Implement
{
    public class ClientService : IClientService
    {
        public IClientRepository _clientRepository;
        public ClientService(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<ClientOutDTO> GetClientListAsync(SelectClientDTO dto)
        {
            return await _clientRepository.GetClientListAsync(dto);
        }
        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<bool> CreateClientAsync(CreateClientDTO dto)
        {
            string clientNo = string.Empty;
            while (true)
            {
                clientNo = Algorithm.GetRandom(8);
                Client data = await _clientRepository.GetEntityAsync(x => x.ClientNo == clientNo);
                if (data is null)
                {
                    break;
                }
            }
            Client client = new Client
            {
                ClientNo = clientNo,
                Name = dto.Name,
                Password = Algorithm.EncryptString(dto.Password),
                Sex = dto.Sex,
                IdNumber = dto.IdNumber,
                BirthDate = dto.BirthDate,
                Address = dto.Address,
                Phone = dto.Phone,
                State = 1,
                CreateTime = DateTime.Now,
                Roles = new List<UserRole> {
                    new UserRole {
                        RoleId = Convert.ToInt32(RolesType.普通用户),
                        UserId = clientNo
                    }
                }
            };
            await _clientRepository.AddEntityAsync(client);
            return await _clientRepository.SaveChangeAsync();
        }
        /// <summary>
        /// 修改部分用户数据
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<bool> UpdateSectionClientAsync(UpdateClientDTO dto)
        {
            Client data = await _clientRepository.GetEntityAsync(x => x.ClientNo == dto.ClientNo);
            if (data is null)
            {
                return false;
            }
            data.State = dto.State;
            data.Phone = dto.Phone;
            return await _clientRepository.SaveChangeAsync();
        }
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<bool> DeleteClientsAsync(string[] clientNos)
        {
            clientNos.ToList().ForEach(async item =>
            {
                var client = await _clientRepository.GetEntityAsync(x => x.ClientNo == item);
                if (!(client is null))
                {
                    _clientRepository.DeleteEntity(client);
                }
            });
            return await _clientRepository.SaveChangeAsync();
        }
        /// <summary>
        /// 修改用户全部数据
        /// </summary>
        /// <param name="file"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<(bool, string)> UpdateAllClientAsync(IFormFile imageFile, UpdateClientDTO dto)
        {
            Client book = await _clientRepository.GetEntityAsync(x => x.ClientNo == dto.ClientNo);
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
                book.HeaderImgSrc = $"/src/Client/HeaderImg/{exist.Item2}";
            }
            book.Name = dto.Name;
            book.Phone = dto.Phone;
            book.Sex = (Sex)dto.Sex;
            return (await _clientRepository.SaveChangeAsync(), "");
        }
        /// <summary>
        /// 保存图片到服务器
        /// </summary>
        /// <param name="imageFile"></param>
        /// <returns></returns>
        public async Task<(bool, string)> SaveImageAsync(IFormFile imageFile)
        {
            string basePath = Environment.CurrentDirectory + @"\wwwroot\Client\HeaderImg\";
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
