using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Music.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Music.Controllers
{
    /// <summary>
    /// 视频切片命令:
    /// 第一步：如果 不是mp4格式视频 ，name.xx 格式视频转换为 mp4 :
    /// ffmpeg -i input.mkv -acodec copy -vcodec copy out.mp4 
    /// 
    /// 第二步：
    /// 将mp4格式转换为ts格式
    /// ffmpeg -y -i 本地视频.mp4 -vcodec copy -acodec copy -vbsf h264_mp4toannexb 转换视频.ts
    /// 
    /// 第三步 （-segment_time 1 指定间隔时长）:
    /// 将ts文件进行切片 后期要根据视频总大小 动态调整切片 时长  文件大小  要保证在很短时间内完成文件加载
    /// ffmpeg -i {本地视频.ts} -c copy -map 0 -f segment -segment_list {视频索引文件.m3u8} -segment_time 1 {前缀}-%03d.ts
    /// ffmpeg -i huran.ts -c copy -map 0 -f segment -segment_list huran.m3u8 -segment_time 1 huran-%03d.ts
    /// 
    /// 
    /// 太慢了：
    /// ffmpeg -i 2.mp4 -hls_time 1 -f hls 2.m3u8
    /// </summary>
    public class VideoController : Controller
    {
        private readonly IWebHostEnvironment _hostEnvironment;

        public VideoController(IWebHostEnvironment environment)
        {
            _hostEnvironment = environment;
        }
        //ffmpeg -y -i 20210606_195836.mp4 -vcodec copy -acodec copy -vbsf h264_mp4toannexb 20210606_195836.ts
        /// ffmpeg -i 20210606_195836.ts -c copy -map 0 -f segment -segment_list huran.m3u8 -segment_time 5 huran-%03d.ts
        public IActionResult Index(string key)
        {
            //DPlayer
            //https://www.helloweba.net/javascript/570.html
            try
            {
                var videoInfo = new VideoInfo();

                //硬编码
                if (!string.IsNullOrWhiteSpace(key))
                {
                    //to do
                    //构造视频数据 传递给前端 
                    //key是路径的一部分 子目录
                    var path = Path.Combine(_hostEnvironment.ContentRootPath, "wwwroot", "video", key);
                    var file = Path.Combine(path, key + ".json");
                    if (System.IO.File.Exists(file))
                    {
                        //我把歌曲信息写到这里面了
                        string data = System.IO.File.ReadAllText(file, Encoding.UTF8);
                        videoInfo = System.Text.Json.JsonSerializer.Deserialize<VideoInfo>(data);
                        //歌曲信息反序列化为videoinfo对象
                    }
                    else
                    {
                        //硬编码
                        if (key.Contains("huran"))
                        {
                            videoInfo.title = "湖上 睡前练会琴 - 忽然（Cover 李志）";
                            videoInfo.url = @"/video/huran/huran.m3u8";
                        }
                    }

                    return View("view", videoInfo);
                }
                //videoInfo.title = "这个视频你看之前要慎重，要想好！";
                //videoInfo.url = "/video/p/64.m3u8";
                return View("view", videoInfo);
            }
            catch (Exception ex)
            {
                throw new Exception("唉呀妈呀，错了错了，我错了，我出错了！" + "\r\n" + ex.Message);
            }

        }
    }
}
