using System.Linq;
using System.Security.Cryptography;
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
        /// MD5计算（16位）
        /// </summary>
        /// <param name="text">要计算其哈希代码的字符串。</param>
        /// <param name="isLower">返回结果是否为小写（默认小写）。</param>
        /// <param name="encoding">字符串编码（默认UTF8）。</param>
        /// <returns>计算所得的哈希代码。</returns>
        // ReSharper disable once InconsistentNaming
        public static string MD5_16(string text, bool isLower = true, Encoding encoding = null) => ComputeHash(System.Security.Cryptography.MD5.Create(), text, isLower, encoding, 4, 8);

        /// <summary>
        /// MD5计算（32位）
        /// </summary>
        /// <param name="text">要计算其哈希代码的字符串。</param>
        /// <param name="isLower">返回结果是否为小写（默认小写）。</param>
        /// <param name="encoding">字符串编码（默认UTF8）。</param>
        /// <returns>计算所得的哈希代码。</returns>
        // ReSharper disable once InconsistentNaming
        public static string MD5(string text, bool isLower = true, Encoding encoding = null) => ComputeHash(System.Security.Cryptography.MD5.Create(), text, isLower, encoding);

        /// <summary>
        /// SHA1计算
        /// </summary>
        /// <param name="text">要计算其哈希代码的字符串。</param>
        /// <param name="isLower">返回结果是否为小写（默认小写）。</param>
        /// <param name="encoding">字符串编码（默认UTF8）。</param>
        /// <returns>计算所得的哈希代码。</returns>
        // ReSharper disable once InconsistentNaming
        public static string SHA1(string text, bool isLower = true, Encoding encoding = null) => ComputeHash(System.Security.Cryptography.SHA1.Create(), text, isLower, encoding);

        /// <summary>
        /// SHA256计算
        /// </summary>
        /// <param name="text">要计算其哈希代码的字符串。</param>
        /// <param name="isLower">返回结果是否为小写（默认小写）。</param>
        /// <param name="encoding">字符串编码（默认UTF8）。</param>
        /// <returns>计算所得的哈希代码。</returns>
        // ReSharper disable once InconsistentNaming
        public static string SHA256(string text, bool isLower = true, Encoding encoding = null) => ComputeHash(System.Security.Cryptography.SHA256.Create(), text, isLower, encoding);

        /// <summary>
        /// SHA384计算
        /// </summary>
        /// <param name="text">要计算其哈希代码的字符串。</param>
        /// <param name="isLower">返回结果是否为小写（默认小写）。</param>
        /// <param name="encoding">字符串编码（默认UTF8）。</param>
        /// <returns>计算所得的哈希代码。</returns>
        // ReSharper disable once InconsistentNaming
        public static string SHA384(string text, bool isLower = true, Encoding encoding = null) => ComputeHash(System.Security.Cryptography.SHA384.Create(), text, isLower, encoding);

        /// <summary>
        /// SHA512计算
        /// </summary>
        /// <param name="text">要计算其哈希代码的字符串。</param>
        /// <param name="isLower">返回结果是否为小写（默认小写）。</param>
        /// <param name="encoding">字符串编码（默认UTF8）。</param>
        /// <returns>计算所得的哈希代码。</returns>
        // ReSharper disable once InconsistentNaming
        public static string SHA512(string text, bool isLower = true, Encoding encoding = null) => ComputeHash(System.Security.Cryptography.SHA512.Create(), text, isLower, encoding);

        /// <summary>
        /// 计算指定字符串的哈希值。
        /// </summary>
        /// <typeparam name="T">哈希算法</typeparam>
        /// <param name="hashAlgorithm">哈希算法实例</param>
        /// <param name="text">要计算其哈希代码的字符串。</param>
        /// <param name="isLower">返回结果是否为小写（默认小写）。</param>
        /// <param name="encoding">字符串编码（默认UTF8）。</param>
        /// <param name="offset">字节数组中的偏移量，从该位置开始使用数据。</param>
        /// <param name="count">数组中用作数据的字节数。</param>
        /// <returns>计算所得的哈希代码。</returns>
        public static string ComputeHash<T>(T hashAlgorithm, string text, bool isLower = true, Encoding encoding = null, int? offset = null, int? count = null) where T : HashAlgorithm
        {
            try
            {
                var data = (encoding ?? Encoding.UTF8).GetBytes(text);
                return string.Join(string.Empty,
                    (offset.HasValue && count.HasValue
                        ? hashAlgorithm.ComputeHash(data).Skip(offset.Value).Take(count.Value)
                        : hashAlgorithm.ComputeHash(data)).Select(x => x.ToString($"{(isLower ? "x" : "X")}2")));
            }
            finally
            {
                hashAlgorithm.Dispose();
            }
        }
    }
}