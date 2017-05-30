using System.Linq;
using System.Text;

// ReSharper disable once CheckNamespace
namespace AigioSoft.Common.Helpers
{
    /// <summary>
    /// 哈希算法
    /// </summary>
    public static class Hashs
    {
        /// <summary>
        /// MD5计算
        /// </summary>
        /// <param name="text">需要计算的值</param>
        /// <param name="isLower">返回结果是否为小写</param>
        /// <returns></returns>
        // ReSharper disable once InconsistentNaming
        public static string MD5(string text, bool isLower = true)
        {
            return string.Join(string.Empty, System.Security.Cryptography.MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(text)).Select(x => x.ToString($"{(isLower ? "x" : "X")}2")));
        }

        /// <summary>
        /// SHA1计算
        /// </summary>
        /// <param name="text">需要计算的值</param>
        /// <param name="isLower">返回结果是否为小写</param>
        /// <returns></returns>
        // ReSharper disable once InconsistentNaming
        public static string SHA1(string text, bool isLower = true)
        {
            return string.Join(string.Empty, System.Security.Cryptography.SHA1.Create().ComputeHash(Encoding.UTF8.GetBytes(text)).Select(x => x.ToString($"{(isLower ? "x" : "X")}2")));
        }

        /// <summary>
        /// SHA256计算
        /// </summary>
        /// <param name="text">需要计算的值</param>
        /// <param name="isLower">返回结果是否为小写</param>
        /// <returns></returns>
        // ReSharper disable once InconsistentNaming
        public static string SHA256(string text, bool isLower = true)
        {
            return string.Join(string.Empty, System.Security.Cryptography.SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(text)).Select(x => x.ToString($"{(isLower ? "x" : "X")}2")));
        }

        /// <summary>
        /// SHA512计算
        /// </summary>
        /// <param name="text">需要计算的值</param>
        /// <param name="isLower">返回结果是否为小写</param>
        /// <returns></returns>
        // ReSharper disable once InconsistentNaming
        public static string SHA512(string text, bool isLower = true)
        {
            return string.Join(string.Empty, System.Security.Cryptography.SHA512.Create().ComputeHash(Encoding.UTF8.GetBytes(text)).Select(x => x.ToString($"{(isLower ? "x" : "X")}2")));
        }
    }
}