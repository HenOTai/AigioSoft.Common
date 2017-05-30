using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

// ReSharper disable once CheckNamespace
namespace AigioSoft.Common
{
    /// <summary>
    /// 标签-低版本IE(不包含IE11)浏览器过滤
    /// </summary>
    public class LowBrowserVerFilterAttribute : ActionFilterAttribute
    {
        public static string LowBrowserVerPagePath = "~/Views/Shared/LowBrowserVer.cshtml";

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var userAgent = context.HttpContext.Request.Headers["User-Agent"].FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(userAgent)
                && userAgent.IndexOf("MSIE", StringComparison.OrdinalIgnoreCase) != -1
                && userAgent.IndexOf("rv:11.0", StringComparison.OrdinalIgnoreCase) == -1)
            {
                context.Result = new ViewResult
                {
                    ViewName = LowBrowserVerPagePath
                };
            }
        }
    }
}
