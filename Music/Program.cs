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
            //7.����(2014 Live i / O ��)


            //var s = "��־ - ����(2014i��O��).mp3";
            //var t1 = s.Trim();
            //var t2 = s.Trim().Replace("��־", "").Replace("-", "");
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
