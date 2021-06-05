using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Music.Controllers
{
    /// <summary>
    /// 视频切片命令:ffmpeg -i {本地视频.ts} -c copy -map 0 -f segment -segment_list {视频索引文件.m3u8} -segment_time 5 {前缀}-%03d.ts
    /// </summary>
    public class VideoController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.url = "";
            return View();
        }
    }
}
