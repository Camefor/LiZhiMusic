using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Music {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            } else {
                app.UseExceptionHandler("/Home/Error");
            }

            var provider = new FileExtensionContentTypeProvider();

            provider.Mappings[".flac"] = "audio/mpeg";
            provider.Mappings[".lrc"] = "application/octet-stream";
            provider.Mappings[".m3u8"] = "application/octet-stream";
            provider.Mappings[".mp3"] = "audio/mpeg";
            provider.Mappings[".ico"] = "image/x-icon";



            var provider2 = new FileExtensionContentTypeProvider();

            provider2.Mappings[".flac"] = "audio/mpeg";
            provider2.Mappings[".lrc"] = "application/octet-stream";
            provider2.Mappings[".mp3"] = "audio/mpeg";
            provider2.Mappings[".ico"] = "image/x-icon";
            app.UseStaticFiles();

            app.UseStaticFiles(new StaticFileOptions {
                //FileProvider = new PhysicalFileProvider(Directory.GetCurrentDirectory()),
                //设置不限制content-type 该设置可以下载所有类型的文件，但是不建议这么设置，因为不安全
                ServeUnknownFileTypes = true

            });
            //app.UseStaticFiles(new StaticFileOptions { 
            //    FileProvider = new PhysicalFileProvider(Path.Combine(env.WebRootPath,"love")),
            //    RequestPath = "/love",
            //    ContentTypeProvider = provider
            //});

            //app.UseStaticFiles(new StaticFileOptions {
            //    FileProvider = new PhysicalFileProvider(Path.Combine(env.WebRootPath, "lyric")),
            //    RequestPath = "/lyric",
            //    ContentTypeProvider = provider2
            //});
            //app.UseStaticFiles(new StaticFileOptions {
            //    FileProvider = new PhysicalFileProvider(Path.Combine(env.WebRootPath , "love")) ,
            //    RequestPath = "/video" ,
            //    ContentTypeProvider = provider
            //});

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
