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

namespace Music.Controllers {
    public class HomeController : Controller {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _hostEnvironment;
        static HtmlParser htmlParser = new HtmlParser();
        static readonly HttpClient client = new HttpClient();
        static HttpWebRequest myReq;
        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment environment) {
            _logger = logger;
            _hostEnvironment = environment;
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

            DownloadLyric();

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
            foreach (FileInfo i in fileinfos) {
                fileInfoModels.Add(new SourceModel {
                    count = fileinfos.Length,
                    name = i.Name,
                    author = "李志",
                    cover = "https://2019334.xyz/share/cover/1.jpg",//后期动态更换专辑图片
                    src = @"../love/" + i.Name,
                    lyric = "http://192.168.2.6:2333/content/temp/res/李志-忽然.lrc?t=" + count++.ToString()
                });
            }

            return fileInfoModels;
        }


        private static void DownloadLyric(string key = "") {

            //保存所有歌词地址
            var sourceHtmlDom = AnalyticalContent.GetHtml("https://www.mulanci.org/lyric/s4127/");
            var dom = htmlParser.ParseDocument(sourceHtmlDom);
            var textItems = dom.QuerySelectorAll("div.pt-1 a");//元素选择器 //pb-1
            List<LyricUrlModel> lyricUrlModels = new List<LyricUrlModel>();
            foreach (var item in textItems) {
                var href = "https://www.mulanci.org/" + item.GetAttribute("href");
                var text = AnalyticalContent.HtmlToPlainText(item.InnerHtml);
                lyricUrlModels.Add(new LyricUrlModel {
                    text = text,
                    url = href
                });
                if (text.Contains(key)) {
                    //haha
                    var find = true;
                }
            }

            var temp = lyricUrlModels.Where(x => x.text.Contains(key)).ToList();
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
