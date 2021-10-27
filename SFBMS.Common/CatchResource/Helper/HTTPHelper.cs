using AngleSharp;
using AngleSharp.Dom;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SFBMS.Common.CatchResource.Helper
{
    public class HTTPHelper
    {
        public static HttpClient Client { get; } = new HttpClient();

        public static string GetHTMLByURL(string url)
        {
            try
            {
                WebRequest wRequest = WebRequest.Create(url);
                wRequest.ContentType = "text/html; charset=gb2312";
                wRequest.Method = "get";
                wRequest.UseDefaultCredentials = true;
                WebResponse wResp = wRequest.GetResponseAsync().Result;
                Stream respStream = wResp.GetResponseStream();
                //dy2018这个网站编码方式是GB2312,
                using StreamReader reader = new StreamReader(respStream, Encoding.GetEncoding("GB2312"));
                return reader.ReadToEnd();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return string.Empty;
            }
        }
        /// <summary>
        /// 获取HTML节点信息
        /// </summary>
        /// <param name="url">网址</param>
        /// <param name="selector">选择器信息</param>
        /// <returns></returns>
        public static async Task<IHtmlCollection<IElement>> GetHTMLDOMByURLAsync(string url,string selector)
        {
            IConfiguration config = Configuration.Default.WithDefaultLoader();
            IBrowsingContext context = BrowsingContext.New(config);
            IHtmlCollection<IElement> elements = (await context.OpenAsync(url)).QuerySelectorAll(selector);
            return elements;
        }
    }
}
