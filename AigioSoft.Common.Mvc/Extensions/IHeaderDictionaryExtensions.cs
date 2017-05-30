using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

// ReSharper disable once CheckNamespace
namespace AigioSoft.Common
{
    /// <summary>
    /// Http Request Header 扩展
    /// </summary>
    public static class HeaderDictionaryExtensions
    {
        /// <summary>
        /// 获取客户端浏览器的原始用户代理信息。
        /// </summary>
        /// <param name="headerDictionary"></param>
        /// <returns></returns>
        public static string UserAgent(this IHeaderDictionary headerDictionary)
        {
            return headerDictionary.TryGetValue("User-Agent", out StringValues value) ? value.ToString() : null;
        }
    }
}
