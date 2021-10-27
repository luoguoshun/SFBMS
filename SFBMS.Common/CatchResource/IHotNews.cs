using AngleSharp.Dom;
using SFBMS.Common.CatchResource.Modles;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SFBMS.Common.CatchResource
{
    public interface IHotNews
    {
        Task<IHtmlCollection<IElement>> GetHTMLDOMByURLAsync(string url, string selector);
        Task<IList<ImageResult>> GetHotNewsByHtmlAgilityAsync();
        Task<IList<ImageResult>> GetImageByAngleSharpAsync(string url);
        Task<IList<MovieResult>> GetMovieByAngleSharpAsync(string url);
    }
}
