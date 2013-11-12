using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CustomImageBrowser.Web
{
    public class ImageModel
    {
        public string Id { get; set; }
        public string OriginalName { get; set; }
        public string Extension { get; set; }
        public string Url { get; set; }
        public string ThumbUrl { get; set; }

    }

    public class ImageBrowserModel
    {
        public int UserId { get; set; }
        public List<ImageModel> Images { get; set; }
            
    }
}