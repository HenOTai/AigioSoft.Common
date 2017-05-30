using System;
using System.Linq;
using Microsoft.AspNetCore.Http;

// ReSharper disable once CheckNamespace
namespace AigioSoft.Common.Helpers
{
    /// <summary>
    /// 功能-Chrome浏览器相关操作
    /// </summary>
    public static class Chrome
    {
        /// <summary>
        /// 用于Chrome56+在http页面中pwd input标记为不安全的解决方案
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string UnsafeInput(HttpContext context)
        {
            if (!context.Request.IsHttps)
            {
                var userAgent = context.Request.Headers["User-Agent"].FirstOrDefault();
                if (!string.IsNullOrWhiteSpace(userAgent))
                {
                    var constStr = "Chrome/5";
                    var index = userAgent.IndexOf(constStr, StringComparison.OrdinalIgnoreCase);
                    if (index != -1 && userAgent.Length > constStr.Length + index + 1)
                    {
                        var ver = userAgent.Substring(index + constStr.Length, 1);
                        int version;
                        if (int.TryParse(ver, out version) && version >= 6)
                        {
                            return "text";
                        }
                    }
                }
            }
            return "password";
        }
    }
}
