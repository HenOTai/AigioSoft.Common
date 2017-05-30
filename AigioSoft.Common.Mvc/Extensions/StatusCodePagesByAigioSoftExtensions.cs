using System;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Diagnostics;
using AigioSoft.Common.Mvc.Properties;

// ReSharper disable once CheckNamespace
namespace Microsoft.AspNetCore.Builder
{
    /// <summary>
    /// 静态错误页面处理扩展
    /// </summary>
    public static class StatusCodePagesByAigioSoftExtensions
    {
        /// <summary>
        /// 添加一个StatusCodePages中间件与指定处理程序检查，响应状态码400~599之间，响应预设静态错误页。
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseStatusCodePagesByAigioSoft(this IApplicationBuilder app) => app
            .UseStatusCodePages(async context =>
            {
                await Handler(context);
            });

        /// <summary>
        /// 添加一个StatusCodePages中间件与指定处理程序检查，响应状态码400~599之间，响应预设静态错误页。
        /// </summary>
        /// <param name="app"></param>
        /// <param name="backgroundColor">预设静态错误页背景颜色</param>
        /// <param name="fontColor">预设静态错误页字体颜色</param>
        /// <returns></returns>
        public static IApplicationBuilder UseStatusCodePagesByAigioSoft(this IApplicationBuilder app, string backgroundColor, string fontColor) => app.UseStatusCodePages(async context =>
            {
                await Handler(context, null, backgroundColor, fontColor);
            });

        /// <summary>
        /// 添加一个StatusCodePages中间件与指定处理程序检查，响应状态码400~599之间，
        /// 响应预设静态错误页，如无静态页面可手动指定返回方式。
        /// </summary>
        /// <param name="app"></param>
        /// <param name="handler">手动指定返回方式</param>
        /// <returns></returns>
        public static IApplicationBuilder UseStatusCodePagesByAigioSoft(this IApplicationBuilder app,
            Func<StatusCodeContext, Task> handler) => app.UseStatusCodePages(async context =>
        {
            await Handler(context, handler);
        });

        /// <summary>
        /// 添加一个StatusCodePages中间件与指定处理程序检查，响应状态码400~599之间，
        /// 响应预设静态错误页，如无静态页面可手动指定返回方式。
        /// </summary>
        /// <param name="app"></param>
        /// <param name="handler">手动指定返回方式</param>
        /// <param name="backgroundColor">预设静态错误页背景颜色</param>
        /// <param name="fontColor">预设静态错误页字体颜色</param>
        /// <returns></returns>
        public static IApplicationBuilder UseStatusCodePagesByAigioSoft(this IApplicationBuilder app,
            Func<StatusCodeContext, Task> handler, string backgroundColor, string fontColor) => app.UseStatusCodePages(async context =>
        {
            await Handler(context, handler, backgroundColor, fontColor);
        });

        /// <summary>
        /// 处理静态错误页面逻辑
        /// </summary>
        /// <param name="context"></param>
        /// <param name="handler"></param>
        /// <param name="backgroundColor"></param>
        /// <param name="fontColor"></param>
        /// <returns></returns>
        private static async Task Handler(StatusCodeContext context, Func<StatusCodeContext, Task> handler = null,
            string backgroundColor = null, string fontColor = null)
        {
            var htmlString = Resources.ResourceManager.GetString("_" + context.HttpContext.Response.StatusCode, CultureInfo.CurrentCulture);
            if (htmlString != null)
            {
                context.HttpContext.Response.ContentType = "text/html";
                if (!string.IsNullOrWhiteSpace(backgroundColor))
                    htmlString = htmlString.Replace("#605ca8", backgroundColor);
                if (!string.IsNullOrWhiteSpace(fontColor))
                    htmlString = htmlString.Replace("#ffffff", fontColor);
                await context.HttpContext.Response.WriteAsync(htmlString);
            }
            else if (handler == null)
            {
                context.HttpContext.Response.ContentType = "text/plain";
                await context.HttpContext.Response.WriteAsync(
                    "状态代码：" + context.HttpContext.Response.StatusCode);
            }
            else
                await handler(context);
        }
    }
}
