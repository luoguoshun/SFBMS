using HtmlAgilityPack;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using SFBMS.Common.CatchResource.Modles;
using AngleSharp;
using AngleSharp.Dom;
using System;
using System.Net;
using System.IO;
using System.Net.Http;

namespace SFBMS.Common.CatchResource.Implement
{
    public class HotNews : IHotNews
    {
        private IHttpClientFactory _clientFactory;
        public HotNews(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }
        public async Task<IList<ImageResult>> GetHotNewsByHtmlAgilityAsync()
        {
            List<ImageResult> list = new List<ImageResult>();

            HtmlWeb web = new HtmlWeb();

            HtmlDocument htmlDocument = await web.LoadFromWebAsync("https://www.cnblogs.com/");

            //主要是拿到a标签
            List<HtmlNode> node = htmlDocument.DocumentNode.SelectNodes("//*[@id='post_list']/article/section/div/a").ToList();
            Parallel.ForEach(node, (item) =>
               {
                   list.Add(new ImageResult
                   {
                       Title = item.InnerText,
                       Url = item.GetAttributeValue("href", "")/*无值返回空*/
                   });
               });
            return list;
        }
        /// <summary>
        /// 获取HTML节点信息
        /// </summary>
        /// <param name="url">网址</param>
        /// <param name="selector">选择器信息</param>
        /// <returns></returns>
        public async Task<IHtmlCollection<IElement>> GetHTMLDOMByURLAsync(string url, string selector)
        {
            IConfiguration config = Configuration.Default.WithDefaultLoader();
            IBrowsingContext context = BrowsingContext.New(config);
            IHtmlCollection<IElement> elements = (await context.OpenAsync(url)).QuerySelectorAll(selector);
            return elements;
        }
        /// <summary>
        /// AngleSharp可以利用css规则去获取数据
        /// </summary>
        /// <returns></returns>
        public async Task<IList<ImageResult>> GetImageByAngleSharpAsync(string url)
        {
            try
            {
                IConfiguration config = Configuration.Default.WithDefaultLoader();
                IBrowsingContext context = BrowsingContext.New(config);
                IHtmlCollection<IElement> cells = await GetHTMLDOMByURLAsync(url, "ul.clearfix>li");

                List<ImageResult> list = new List<ImageResult>();
                foreach (var item in cells)
                {
                    //GetAttribute(string AttributeName)获取节点属性值
                    string imageUrl = item.BaseUri + item.QuerySelector("a>span>img").GetAttribute("src");
                    list.Add(new ImageResult
                    {
                        Title = item.TextContent,
                        Url = imageUrl
                    });
                    await SaveImageAsync(imageUrl);
                }
                return list;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                throw;
            }
        }
        /// <summary>
        /// 打开网址抓取电影信息
        /// </summary>
        /// <param name="url">链接</param>
        /// <returns></returns>
        public async Task<IList<MovieResult>> GetMovieByAngleSharpAsync(string url)
        {
            try
            {
                List<MovieResult> list = new List<MovieResult>();
                IHtmlCollection<IElement> elements = await GetHTMLDOMByURLAsync(url, "div.co_content8");
                foreach (var item in elements)
                {             
                    List<IElement> targetElement = item.QuerySelectorAll("a").Where(a => a.GetAttribute("href").Contains("/html/") && !a.InnerHtml.Contains("最新电影下载") && !a.InnerHtml.Contains("游戏")).ToList();
                    targetElement.ForEach(async a =>
                        {
                            var onlineURL = a.BaseUri + a.GetAttribute("href");
                            MovieResult movieInfo = await GetMovieInfoFormWebAsync(onlineURL.Replace("/index.html", ""));
                            //下载链接不为空才添加到现有数据
                            if (movieInfo is null||movieInfo.DownLoadURL != null)
                            {
                                list.Add(movieInfo);
                            }
                        });
                }
                return list;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                throw;
            }
        }
        /// <summary>
        /// 下载图片
        /// </summary>
        /// <param name="name"></param>
        /// <param name="requestUriString"></param>
        /// <returns></returns>
        private async Task SaveImageAsync(string requestUriString)
        {
            string extension = Path.GetExtension(requestUriString);//拓展名
            string imgExtension = ".bmp .jpg .png .tif .gif .pcx .tga .exif .fpx .svg .psd .cdr .pcd .dxf .ufo .eps .ai .raw .WMF";
            if (imgExtension.Contains(extension))
            {
                string name = Path.GetFileName(requestUriString);
                var savePath = Path.Combine($@"{Environment.CurrentDirectory}\wwwroot\CatchResource\images\", name);//保存路径
                if (!File.Exists(savePath))
                {
                    WebClient mywebclient = new WebClient();
                    await mywebclient.DownloadFileTaskAsync(requestUriString, savePath);
                }
            }
            #region 注释
            //if (!(requestUriString is null))
            //{
            //    //1、首先获取图片的二进制数组。
            //    byte[] b = null;
            //    HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(requestUriString);
            //    using WebResponse myResp = myReq.GetResponse();
            //    using (BinaryReader br = new BinaryReader(myResp.GetResponseStream()))
            //    {
            //        b = br.ReadBytes(500000);
            //    }
            //    //2、保存到磁盘文件中.
            //    var fielName = $"{DateTime.Now:yyyyMMddHHmmss}.jpg";
            //    var savePath = Path.Combine($@"{Environment.CurrentDirectory}\wwwroot\CatchResource\images\", fielName);//保存路径
            //    if (!File.Exists(savePath))
            //    {
            //        using FileStream fs = new FileStream(savePath, FileMode.Create);
            //        using BinaryWriter w = new BinaryWriter(fs);
            //        w.Write(b);
            //    }
            //}
            #endregion
        }
        /// <summary>
        /// 根据链接跳转获取影详细信息
        /// </summary>
        /// <param name="a"></param>
        /// <param name="onlineURL"></param>
        /// <returns></returns>
        private async Task<MovieResult> GetMovieInfoFormWebAsync(string onlineURL)
        {
            IHtmlCollection<IElement> elements = await GetHTMLDOMByURLAsync(onlineURL, "div#Zoom");
            if (elements.Length!=0)
            {
                //下载链接在 span>a中，有可能有多个链接
                IElement downLoadURL = elements.FirstOrDefault().QuerySelector("span>a");
                if (downLoadURL != null)
                {
                    var movieInfo = new MovieResult()
                    {
                        //InnerHtml中可能还包含font标签，做多一个Replace
                        MovieName = "",
                        Url = onlineURL,
                        MovieInfo = "",
                        DownLoadURL = downLoadURL.GetAttribute("href"),//可能没有下载链接
                        ReleaseTime = DateTime.Now.ToString(),

                    };
                    return movieInfo;
                }
            }       
            return null;

        }
    }
}
