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
namespace Music.Controllers {
    public class HomeController : Controller {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger) {
            _logger = logger;
        }

        public IActionResult Index() {
            List<SourceModel> sourceModels = new List<SourceModel>();
            SourceModel sourceModel = new SourceModel();
            string path = @"E:\love";
            return View(GetDirectory(path));
        }
        public static List<FileInfoModel> GetDirectory(string srcPath) {
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

        /// <summary>
        /// 将B转换为MB
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
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
