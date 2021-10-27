using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SFBMS.Common.EnumList;
using SFBMS.Contracts.SystemModule;
using SFBMS.Entity.SystemModule;
using SFBMS.Repository.SystemModule;
using SFBMS.WebAPI.Controllers.Base;
using static SFBMS.Common.EnumList.AppTypes;

namespace SFBMS.WebAPI.Controllers.SystemModule
{
    public class AccountController : BaseController
    {

        #region 构造函数
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IAdminRepository _adminRepository;

        public AccountController(IHttpClientFactory httpClientFactory, IConfiguration configuration, IAdminRepository adminRepository = null)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _adminRepository = adminRepository;
        }
        #endregion
        private string IdentityServerAddress => _configuration.GetSection("IdentityServer").GetValue<string>("Address");

        [HttpPost]
        [ApiExplorerSettings(GroupName = "Background")]
        public async Task<IActionResult> AdminAccount(LoginDTO user)
        {
            //设置请求范围(范围在Sunflower客服端定义请求范围内)
            string Scope = $"{AppTypes.Client.ToString().ToLower()} {AppTypes.Background.ToString().ToLower()}";
            var tokenResponse = await new HttpClient().RequestPasswordTokenAsync(new PasswordTokenRequest
            {
                Address = IdentityServerAddress + "/connect/token",
                ClientId = "Sunflower",
                ClientSecret = "secret",
                Scope = Scope,
                UserName = user.Account,
                Password = user.Password
            });
            if (tokenResponse.IsError)
            {
                if (tokenResponse.ErrorDescription.Equals("invalid_username_or_password"))
                    return JsonFailt("用户名或密码错误，请重新输入");
                else
                    return JsonFailt($"[令牌响应错误]: {tokenResponse.Error}");
            }
            return JsonSuccess("登入成功！", tokenResponse.AccessToken);
        }

        [HttpPost]
        [ApiExplorerSettings(GroupName = "Client")]
        public async Task<IActionResult> DoctorAccount(LoginDTO user)
        {
            //request assess token - 请求访问令牌
            var tokenResponse = await new HttpClient().RequestPasswordTokenAsync(new PasswordTokenRequest
            {
                Address = IdentityServerAddress + "/connect/token",
                ClientId = "Sunflower",
                ClientSecret = "secret",
                Scope = AppTypes.Client.ToString().ToLower(),
                UserName = user.Account,
                Password = user.Password
            });
            if (tokenResponse.IsError)
            {
                if (tokenResponse.ErrorDescription.Equals("invalid_username_or_password"))
                    return JsonFailt("用户名或密码错误，请重新输入");
                else
                    return JsonFailt($"[令牌响应错误]: {tokenResponse.Error}");
            }
            return JsonSuccess("登入成功！", tokenResponse.AccessToken);
        }
    }
}

