using Microsoft.AspNetCore.Authorization;
using SFBMS.Common.EnumList;
using System;
using System.Collections.Generic;
using System.Text;
using static SFBMS.Common.EnumList.AppTypes;

namespace SFBMS.WebAPI.Infrastructure.PolicyHelper
{
    public class PolicyRequirement : IAuthorizationRequirement
    {
        public IEnumerable<AppTypes> AppTypes { get; }
        //params关键字可以指定一维数组数目可变的参数
        //如果未发送任何参数，则params列表的长度为零
        public PolicyRequirement(params AppTypes[] appTypes)
        {
            AppTypes = appTypes;
        }
    }
}
