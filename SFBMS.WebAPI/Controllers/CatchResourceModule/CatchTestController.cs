using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SFBMS.Common.CatchResource;
using SFBMS.Common.CatchResource.Modles;
using SFBMS.WebAPI.Controllers.Base;

namespace SFBMS.WebAPI.Controllers.CatchResourceModule
{
    public class CatchTestController : BaseController
    {
        public IHotNews _hotNews;
        public CatchTestController(IHotNews hotNews)
        {
            _hotNews = hotNews;
        }
        [HttpPost]
        [ApiExplorerSettings(GroupName = "Background")]
        public async Task<IActionResult> Test()
        {
            IList<ImageResult> data = await _hotNews.GetHotNewsByHtmlAgilityAsync();
            return JsonSuccess("",data);
        }
        [HttpPost]
        [ApiExplorerSettings(GroupName = "Background")]
        public async Task<IActionResult> GetImageByAngleSharp()
        {
            string url= "https://pic.netbian.com";
            IList<ImageResult> data = await _hotNews.GetImageByAngleSharpAsync(url);
            return JsonSuccess("", data);
        }
        [HttpPost]
        [ApiExplorerSettings(GroupName = "Background")]
        public async Task<IActionResult> GetMovieByAngleSharp()
        {
            string url = "https://www.ygdy8.com/index.html";
            IList<MovieResult> data = await _hotNews.GetMovieByAngleSharpAsync(url);
            return JsonSuccess("", data);
        }
    }
}
