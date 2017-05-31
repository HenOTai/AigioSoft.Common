using System;

// ReSharper disable once CheckNamespace
namespace AigioSoft.Common.Helpers
{
    /// <summary>
    /// 功能-路径操作
    /// </summary>
    public static class Server
    {

#if DEBUG
        private const string DetectionPath = @"\bin\Debug";
#else
        private const string DetectionPath = @"\bin\Release";
#endif

        /// <summary>
        /// 获取当前站点基目录
        /// </summary>
        public static string BaseDirectory
        {
            get
            {

                var baseDirectory =
#if NET451
                    AppDomain.CurrentDomain.BaseDirectory;
#else
                AppContext.BaseDirectory;
#endif
                var index = baseDirectory.LastIndexOf(DetectionPath, StringComparison.OrdinalIgnoreCase);
                if (index != -1)
                    baseDirectory = baseDirectory.Substring(0, index);
                return baseDirectory;
            }
        }

        /// <summary>
        /// 获取当前站点wwwroot目录
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public static string wwwrootDirectory => System.IO.Path.Combine(BaseDirectory, "wwwroot");

        /// <summary>
        /// 将请求的 URL 中的虚拟路径映射到服务器上的物理路径。
        /// </summary>
        /// <param name="virtualPath"></param>
        /// <returns></returns>
        public static string GetMapPath(string virtualPath)
        {
            if (string.IsNullOrWhiteSpace(virtualPath))
                return string.Empty;
            if (virtualPath.StartsWith("~/"))
                return virtualPath.Replace("~/", BaseDirectory).Replace("/", "\\");
            return System.IO.Path.Combine(BaseDirectory, virtualPath.Replace("/", "\\"));
        }
    }
}
