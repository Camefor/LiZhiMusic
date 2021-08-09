using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Music.Middleware
{
    public static class WebCoreExtensions
    {
        public const string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";


        /// <summary>
        /// 添加跨域策略，从appsetting中读取配置
        /// 封装了跨域策略AddCorsPolicy
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddCorsPolicy(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins,
                builder =>
                {
                    string cop = "*,http://localhost:18579";
                    builder.WithOrigins(cop.Split(","))
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
            });
            /**
             * // 配置 CORS 授权策略
        services.AddCors(options => options.AddPolicy(_defaultCorsPolicyName,
            builder => builder.WithOrigins(
                    Configuration["Application:CorsOrigins"]
                    .Split(",", StringSplitOptions.RemoveEmptyEntries).ToArray()
                )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()));

作者：墨墨墨墨小宇
链接：https://juejin.im/post/5d3bfcba51882503ea1c6e78
来源：掘金
著作权归作者所有。商业转载请联系作者获得授权，非商业转载请注明出处。
             * **/
            return services;
        }


    }
    public class CorsMiddleware
    {
        private readonly RequestDelegate next;

        public CorsMiddleware(RequestDelegate next)
        {
            this.next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Headers.ContainsKey(CorsConstants.Origin))
            {
                context.Response.Headers.Add("Access-Control-Allow-Origin", context.Request.Headers["Origin"]);
                context.Response.Headers.Add("Access-Control-Allow-Methods", "PUT,POST,GET,DELETE,OPTIONS,HEAD,PATCH");
                context.Response.Headers.Add("Access-Control-Allow-Headers", context.Request.Headers["Access-Control-Request-Headers"]);
                context.Response.Headers.Add("Access-Control-Allow-Credentials", "true");

                if (context.Request.Method.Equals("OPTIONS"))
                {
                    context.Response.StatusCode = StatusCodes.Status200OK;
                    return;
                }
            }

            await next(context);
        }
    }
}
