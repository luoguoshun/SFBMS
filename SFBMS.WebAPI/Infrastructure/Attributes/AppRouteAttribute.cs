using Microsoft.AspNetCore.Mvc;
using SFBMS.Common.EnumList;
using static SFBMS.Common.EnumList.AppTypes;

namespace OHEXML.Infrastructure.Attributes
{
    public class AppRouteAttribute : RouteAttribute
    {
        public AppRouteAttribute(AppTypes appType) : base($"/api/{appType}/[controller]/[action]")
        {
            AppType = appType.ToString();
        }
        public string AppType { get; }
    }
}
