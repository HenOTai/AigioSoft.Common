using System.Security.Claims;

// ReSharper disable once CheckNamespace
namespace Microsoft.AspNetCore.Identity
{
    /// <summary>
    /// 扩展-Identity 获取用户主键
    /// </summary>
    public static class IdentityExtension
    {
        /// <summary>
        /// 获取用户主键
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static string GetUserId(this ClaimsPrincipal user) => user.Identity.IsAuthenticated ? user.FindFirst(ClaimTypes.NameIdentifier)?.Value : null;
    }
}
