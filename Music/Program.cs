using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Music {
    public class Program {
        public static void Main(string[] args) {
            //
            //7.妈妈(2014 Live i / O 版)


            //var s = "李志 - 妈妈(2014i／O版).mp3";
            //var t1 = s.Trim();
            //var t2 = s.Trim().Replace("李志", "").Replace("-", "");
            //var t3 = s.Trim().IndexOf("-");

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
