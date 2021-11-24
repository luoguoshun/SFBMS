using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OHEXML.Infrastructure.Attributes;
using SFBMS.Common.EnumList;
using SFBMS.Contracts.ClientModule;
using SFBMS.Service.ClientModule;
using SFBMS.WebAPI.Controllers.Base;

namespace SFBMS.WebAPI.Controllers.ClientModule
{
    public class ClientController : BaseController
    {
        public readonly IClientService _clientService;
        public ClientController(IClientService clientService)
        {
            _clientService = clientService;
        }
        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [AppAuthorize(AppTypes.Background)]
        public async Task<IActionResult> GetClientList(SelectClientDTO dto)
        {
            var clients = await _clientService.GetClientListAsync(dto);
            if (clients is null)
            {
                return JsonFailt("");
            }
            return JsonSuccess(clients);
        }
        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [AppAuthorize(AppTypes.Background)]
        public async Task<IActionResult> CreateClient(CreateClientDTO dto)
        {
            bool result = await _clientService.CreateClientAsync(dto);
            if (result)
            {
                return JsonSuccess("添加成功");
            }
            return JsonFailt("添加失败");
        }
        /// <summary>
        /// 修改用户部分数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AppAuthorize(AppTypes.Background)]
        public async Task<IActionResult> UpdateSectionClient(UpdateClientDTO dto)
        {
            bool result = await _clientService.UpdateSectionClientAsync(dto);
            if (result)
            {
                return JsonSuccess("修改条目成功");
            }
            return JsonFailt("修改条目失败");
        }
        /// <summary>
        /// 修改用户全部数据
        /// </summary>
        /// <param name="file"></param>
        /// <param name="bookId">s书籍编号</param>
        /// <returns></returns>
        [HttpPost]
        [AppAuthorize(AppTypes.Background)]
        public async Task<IActionResult> UpdateAllBook()
        {
            //接受图片
            IFormFile imageFile = Request.Form.Files.FirstOrDefault();
            UpdateClientDTO dto = new UpdateClientDTO();
            PropertyInfo[] properties = dto.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            properties.ToList().ForEach(item =>
            {
                //首字母小写
                string proName = item.Name[0].ToString().ToLower() + item.Name.Substring(1);
                //获取前端传来Formdata里的值
                var data = Request.Form[proName];
                object value = Convert.ChangeType(data.ToString(), item.PropertyType);
                item.SetValue(dto, value, null);
            });
            var result = await _clientService.UpdateAllClientAsync(imageFile, dto);
            if (result.Item1)
            {
                return JsonSuccess("删修改功");
            }
            return JsonFailt(result.Item2);
        }
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [AppAuthorize(AppTypes.Background)]
        public async Task<IActionResult> DeleteClients(DeleteClientDTO dto)
        {
            bool result = await _clientService.DeleteClientsAsync(dto.ClientNos);
            if (result)
            {
                return JsonSuccess("删除成功");
            }
            return JsonFailt("删除失败");
        }
    }
}
