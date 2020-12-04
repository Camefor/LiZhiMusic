using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Music.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using System.Web;
using Music.Helper;
using AngleSharp.Html.Parser;
using System.Net.Http;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;

namespace Music.Controllers {
    public class HomeController : Controller {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _hostEnvironment;
        private static IWebHostEnvironment HostEnvironment;
        static HtmlParser htmlParser = new HtmlParser();
        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment environment) {
            _logger = logger;
            _hostEnvironment = environment;
            HostEnvironment = environment;
        }


        public IActionResult Index() {
            _logger.LogWarning("haha");
            List<SourceModel> sourceModels = new List<SourceModel>();
            SourceModel sourceModel = new SourceModel();
            return View();
        }

        public void WriteLog() {
            var p = Path.Combine(_hostEnvironment.ContentRootPath, "wwwroot", "love");

        }

        [HttpGet]
        public JsonResult Love() {
            var path = Path.Combine(_hostEnvironment.ContentRootPath, "wwwroot", "love");
            var sourceModels = GetSourceList(path);
            for (int i = 0; i < sourceModels.Count; i++) {

            }


            return Json(sourceModels);
        }

        private static List<FileInfoModel> GetFilesInfo(string srcPath = @"E:\love") {
            DirectoryInfo dir = new DirectoryInfo(srcPath);
            var fileinfos = dir.GetFiles();  //获取目录下（不包含子目录）的文件和子目录
            List<FileInfoModel> fileInfoModels = new List<FileInfoModel>();
            foreach (FileInfo i in fileinfos) {
                fileInfoModels.Add(new FileInfoModel {
                    Name = i.Name,
                    FullName = "./" + i.Name,
                    MB = GetMB(Convert.ToDouble(i.Length)),
                    CreationTime = i.CreationTime,
                    LastAccessTime = i.LastAccessTime
                });
            }
            //var res = JsonSerializer.Serialize(fileInfoModels);
            return fileInfoModels;
        }

        private static List<SourceModel> GetSourceList(string srcPath) {
            DirectoryInfo dir = new DirectoryInfo(srcPath);
            var fileinfos = dir.GetFiles();  //获取目录下（不包含子目录）的文件和子目录
            List<SourceModel> fileInfoModels = new List<SourceModel>();
            int count = 0;


            List<LyricUrlModel> lyricUrls = new List<LyricUrlModel>();

            //考虑把歌词地址持久化 避免每次去请求
            var _saveUrlPath = Path.Combine(HostEnvironment.ContentRootPath, "wwwroot", @$"lyricUrls.json");
            if (System.IO.File.Exists(_saveUrlPath)) {
                string readText = System.IO.File.ReadAllText(_saveUrlPath);
                //已经存在
                lyricUrls = JsonSerializer.Deserialize<List<LyricUrlModel>>(readText);
            } else {
                //不存在 请求 并保存
                lyricUrls = GetLyricUrls();
                WriteContent(JsonSerializer.Serialize(lyricUrls), _saveUrlPath);//写入文件
            }


            for (int i = 0; i < fileinfos.Length; i++) {
                var songName = fileinfos[i].Name
                    .Trim()
                    .Replace("李志", "")
                    .Replace("-", "")
                    .Replace(".", "")
                    .Replace("mp3", "")
                    .Replace("flac", "")
                    .Replace(" ", "")
                    .Replace("（", "(").Replace("）", ")")
                    .Replace("   ", "");
                songName = Regex.Replace(songName, @"\{.*\}", "");//过滤{}｛｝
                songName = Regex.Replace(songName, @"\(.*\)", "");//过滤{}｛｝
                songName = Regex.Replace(songName, @"\（.*\）", "");//过滤{}｛｝
                songName = Regex.Replace(songName.Replace("（", "(").Replace("）", ")"), @"\([^\(]*\)", "");
                songName = Regex.Replace(songName, @"\d", "");
                if (fileinfos[i].Name.Contains("离婚")) {
                    var sd = "";
                }

                //拿到每首歌曲的歌词地址
                //var _lyricUrl = string.Empty;
                //var _lyricText = string.Empty;
                //var _lyricObj = lyricUrls.Where(x => x.text.Contains(songName)).FirstOrDefault();
                //if (_lyricObj != null) {
                //    _lyricUrl = _lyricObj.url;
                //    _lyricText = DownloadLyric(_lyricUrl);

                //    var _path = Path.Combine(HostEnvironment.ContentRootPath, "wwwroot", "lyric", "res", @$"{ _lyricObj.text.Replace(@"\", "")}.lrc");

                //    WriteContent(_lyricText, _path);//写入文件
                //}
                /**
                 * 
                 *
                 *[ti:忽然]
                  [ar:李志]
                  [al:你好，郑州]
                  [by:2369972023]
                  [00:00.00]歌词千寻 www.lrcgc.com
                  [00:00.62]忽然 - 李志
                  [00:02.44]忽然就流出泪来
                  [00:05.32]忽然想要听到她的声音
                  [00:09.37]而我却什么话都说不出来
                  [00:14.68]是谁在温暖你
                  [00:17.69]有谁会让我觉得
                  [00:20.19]这夜晚还有期盼
                  [00:23.91]我就会跟着它去远行
                  [00:27.09]
                  [00:39.51]可是你在哪里
                  [00:42.68]可是明天醒来的第一脸阳光
                  [00:47.01]是否 会像梦里一样明亮
                  [00:52.04]幻想朝西的生活
                  [00:55.17]幻想着你被害怕定格的角落
                  [00:59.45]最后 我一个人越走越孤单
                  [01:04.58]幻想朝西的生活
                  [01:07.62]幻想着你被灯光伤感了寂寞
                  [01:11.84]最后 你一个人越走越孤单
                  [01:17.05]害怕朝西的生活
                  [01:20.08]害怕着你被灯光伤感了寂寞
                  [01:24.41]最后 我们就越走越孤单
                  [01:29.86]lrc_by:喏小七 QQ_2369972023
                  
                  找歌词，上歌词千寻 www.lrcgc.com。支持歌词找歌名，LRC歌词免费下载。
                 * 
                 * **/
                fileInfoModels.Add(new SourceModel {
                    count = fileinfos.Length,//总数
                    name = fileinfos[i].Name,
                    author = "李志",
                    cover = "https://2019334.xyz/share/cover/1.jpg",//后期动态更换专辑图片
                    src = @"../love/" + fileinfos[i].Name,
                    //lyric = "http://192.168.2.6:2333/content/temp/res/李志-忽然.lrc?t=" + count++.ToString()
                    lyric = "http://192.168.2.6:2333/content/temp/res/意味-李志.lrc?t=" + count++.ToString()
                });
            }

            return fileInfoModels;
        }

        private static void WriteContent(string content, string path) {
            if (!System.IO.File.Exists(path)) {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(path, append: true)) {
                    file.WriteLine(content);
                }
            }
        }




        private static void WriteText(string content, string path) {
            // 此文本只添加到文件一次。
            if (!System.IO.File.Exists(path)) {
                // 创建要写入的文件。
                System.IO.File.WriteAllText(path, content);
                System.IO.File.AppendAllText(path, "\r\n");
            }
            // 这个文本总是被添加，使文件随着时间的推移而变长
            // 如果它没有被删除。
            System.IO.File.AppendAllText(path, content);
        }

        private static void ReadText(string path) {
            FileStream fileStream = new FileStream(path, FileMode.Open);
            using (StreamReader reader = new StreamReader(fileStream)) {
                string line = reader.ReadLine();
            }
        }


        /// <summary>
        /// 获取所有歌词地址
        /// </summary>
        /// <returns></returns>
        private static List<LyricUrlModel> GetLyricUrls() {
            /**
             * 李志( Li Zhi )
             * 常用名：逼哥, Li Zhi, 李志
             * 共收录20张专辑，203篇歌词。
             * **/
            Thread.Sleep(300);
            var url = "https://www.mulanci.org/lyric/s4127/";
            var sourceHtmlDom = AnalyticalContent.GetHtml(url);
            var dom = htmlParser.ParseDocument(sourceHtmlDom);
            var textItems = dom.QuerySelectorAll("div.pt-1 a");//元素选择器 //pb-1
            List<LyricUrlModel> lyricUrlModels = new List<LyricUrlModel>();
            foreach (var item in textItems) {

                var songName = item.InnerHtml
                   .Trim()
                   .Replace("李志", "")
                   .Replace("-", "")
                   .Replace(".", "")
                   .Replace(" ", "")
                   .Replace("(2014 Live i / O 版)", "")
                   .Replace("   ", "");
                songName = Regex.Replace(songName, @"\{.*\}", "");//过滤{}｛｝
                songName = Regex.Replace(songName, @"\（.*\）", "");//过滤{}｛｝
                songName = Regex.Replace(songName, @"\(.*\)", "");//过滤{}｛｝
                songName = Regex.Replace(songName, @"\d", "");


                lyricUrlModels.Add(new LyricUrlModel {
                    text = songName,
                    url = "https://www.mulanci.org/" + item.GetAttribute("href")
                });
            }
            return lyricUrlModels;
        }

        /// <summary>
        /// 下载歌词
        /// </summary>
        /// <param name="url">歌词地址</param>
        private static string DownloadLyric(string url = "https://www.mulanci.org/lyric/sl105975/") {
            Thread.Sleep(300);
            var res = "";
            var sourceHtmlDom = AnalyticalContent.GetHtml(url);
            var dom = htmlParser.ParseDocument(sourceHtmlDom);
            var textItems = dom.QuerySelectorAll("div#lyric-content");//元素选择器 //pb-1
            foreach (var item in textItems) {
                var text = item.InnerHtml;
                var t1 = text.Replace("<br>", "\r\n");
                var sd = t1.IndexOf("<div");
                res = t1.Substring(0, sd - 1);
                res = res.Replace("        作词：李志", "作词：李志");
            }
            return res;
        }


        /// <summary>
        /// 获取所有的专辑封面
        /// </summary>
        /// <param name="srcPath"></param>
        /// <returns></returns>
        private static List<FileInfoModel> GetCover(string srcPath = @"E:\cover") {
            DirectoryInfo dir = new DirectoryInfo(srcPath);
            var fileinfos = dir.GetFiles();  //获取目录下（不包含子目录）的文件和子目录
            List<FileInfoModel> fileInfoModels = new List<FileInfoModel>();
            foreach (FileInfo i in fileinfos) {
                fileInfoModels.Add(new FileInfoModel {
                    Name = i.Name,
                    FullName = "./" + i.Name + i.Extension,
                    MB = GetMB(Convert.ToDouble(i.Length)),
                    CreationTime = i.CreationTime,
                    LastAccessTime = i.LastAccessTime
                });
            }
            //var res = JsonSerializer.Serialize(fileInfoModels);
            return fileInfoModels;
        }

        private static string GetMB(double size) {
            String[] units = new String[] { "B", "KB", "MB", "GB", "TB", "PB" };
            double mod = 1024.0;
            int i = 0;
            while (size >= mod) {
                size /= mod;
                i++;
            }
            return Math.Round(size) + units[i];
        }

        public IActionResult Privacy() {
            return View();
        }

        public IActionResult Album() {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
