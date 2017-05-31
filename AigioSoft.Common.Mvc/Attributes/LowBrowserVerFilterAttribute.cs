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
        public LowBrowserVerFilterAttribute()
        {

        }

        public LowBrowserVerFilterAttribute(string lowBrowserVerPagePath)
        {
            LowBrowserVerPagePath = lowBrowserVerPagePath;
        }

        public string LowBrowserVerPagePath { get; }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var userAgent = context.HttpContext.Request.Headers["User-Agent"].FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(userAgent)
                && userAgent.IndexOf("MSIE", StringComparison.OrdinalIgnoreCase) != -1
                && userAgent.IndexOf("rv:11.0", StringComparison.OrdinalIgnoreCase) == -1)
            {
                if (string.IsNullOrWhiteSpace(LowBrowserVerPagePath))
                {
                    context.Result = new ContentResult
                    {
                        Content = Mvc.Properties.Resources.LowBrowserVer,
                        StatusCode = 200,
                        ContentType = "text/html"
                    };
                }
                else
                {
                    var lastIndex = LowBrowserVerPagePath.LastIndexOf('.') + 1;
                    if (lastIndex > 0)
                    {
                        var extension =
                            LowBrowserVerPagePath.Substring(lastIndex, LowBrowserVerPagePath.Length - lastIndex);
                        if (string.Equals(extension, "cshtml", StringComparison.OrdinalIgnoreCase) ||
                            string.Equals(extension, "html", StringComparison.OrdinalIgnoreCase))
                        {
                            context.Result = new ViewResult
                            {
                                ViewName = LowBrowserVerPagePath,
                                StatusCode = 200
                            };
                        }
                        else
                        {
                            throw new ArgumentOutOfRangeException(nameof(LowBrowserVerPagePath));
                        }
                    }
                }
            }
        }
    }
}
