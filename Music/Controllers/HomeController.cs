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

namespace Music.Controllers {
    public class HomeController : Controller {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _hostEnvironment;

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
            return Json(GetSourceList(path));
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
            foreach (FileInfo i in fileinfos) {
                fileInfoModels.Add(new SourceModel {
                    count = fileinfos.Length,
                    name = i.Name,
                    author = "李志",
                    cover = "https://2019334.xyz/share/cover/1.jpg",
                    src = "./love/" + i.Name
                    //src = "./love/" + HttpUtility.UrlEncode(i.Name)
                    //src = "https://2019334.xyz/share/1.%20%E8%A2%AB%E7%A6%81%E5%BF%8C%E7%9A%84%E6%B8%B8%E6%88%8F%282004%29/01黑色信封.mp3"
                });
            }
            //var res = JsonSerializer.Serialize(fileInfoModels);
            return fileInfoModels;
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
