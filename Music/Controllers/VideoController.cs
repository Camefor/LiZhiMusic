using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Music.Controllers
{
    public class VideoController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.url = "";
            return View();
        }
    }
}
