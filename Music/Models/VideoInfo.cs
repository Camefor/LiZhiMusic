using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Music.Models
{
    public class VideoInfo
    {
        public string name { get; set; }

        /// <summary>
        /// 视频地址 视频链接
        /// </summary>
        public string url { get; set; }

        /// <summary>
        /// 页面顶部title介绍
        /// </summary>
        public string title { get; set; }

        /// <summary>
        /// 视频封面

        /// </summary>
        public string pic { get; set; } = "https://2019334.xyz/share/cover/2.jpg";

        /// <summary>
        /// 视频缩略图，可以使用 DPlayer-thumbnails 生成
        /// </summary>
        public string thumbnails { get; set; }
    }
}
