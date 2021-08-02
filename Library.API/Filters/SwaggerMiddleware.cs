using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NLog.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Library.API.Filters
{
    /// <summary>
    /// 
    /// </summary>
    public class SwaggerAuthorizedMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _safelist;
        private readonly ILogger<SwaggerAuthorizedMiddleware> _logger;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="next"></param>
        /// <param name="safelist"></param>
        /// <param name="logger"></param>
        public SwaggerAuthorizedMiddleware(RequestDelegate next, string safelist, ILogger<SwaggerAuthorizedMiddleware> logger)
        {
            _next = next;
            _safelist = safelist;
            _logger = logger;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            var logger = NLogBuilder.ConfigureNLog("Nlog.config").GetCurrentClassLogger();
            logger.Debug($"ip: {context.Connection.RemoteIpAddress.ToString()}");
            var addresses = _safelist.Split(";");

            if (context.Request.Path.StartsWithSegments("/swagger") && !addresses.Any(a=>a == context.Connection.RemoteIpAddress.ToString()))
            {

                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;

                
            }

            await _next.Invoke(context);

        }
    }
    /// <summary>
    /// 
    /// </summary>
    public static class SwaggerAuthorizeExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseSwaggerAuthorized(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<SwaggerAuthorizedMiddleware>();
        }
    }
}
