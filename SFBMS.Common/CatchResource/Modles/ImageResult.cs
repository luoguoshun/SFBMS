using System;
using System.Collections.Generic;
using System.Text;

namespace SFBMS.Common.CatchResource.Modles
{
    public class ImageResult
    {
        public string Title { get; set; }
        public string Url { get; set; }
    }
    public class MovieResult
    {
        public string MovieName { get; set; }
        public string Url { get; set; }
        public string MovieInfo { get; set; }
        public string ReleaseTime { get; set; }
        public string DownLoadURL { get;  set; }
    }
}
