using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace AigioSoft.Common.Helpers
{
    /// <summary>
    /// RSA操作
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public static class RSA
    {
        /// <summary>
        /// RSA解密（十六进制密文格式）
        /// </summary>
        /// <param name="privatekey">RSA私钥</param>
        /// <param name="content">密文</param>
        /// <returns></returns>
        public static string DecryptHex(string privatekey, params string[] content) => Decrypt(privatekey, null,
            x => x.HexStringToBytes(), content);

        /// <summary>
        /// RSA解密（十六进制密文格式）
        /// </summary>
        /// <param name="rsa">RSA私钥</param>
        /// <param name="content">密文</param>
        /// <returns></returns>
        public static string DecryptHex(RSACryptoServiceProvider rsa, params string[] content) => Decrypt(rsa, null,
            x => x.HexStringToBytes(), content);

        /// <summary>
        /// RSA解密
        /// </summary>
        /// <param name="privatekey">RSA私钥</param>
        /// <param name="content">密文</param>
        /// <returns></returns>
        public static string Decrypt(string privatekey, params string[] content) => Decrypt(privatekey, null,
            null, content);

        /// <summary>
        /// RSA解密
        /// </summary>
        /// <param name="rsa">RSA私钥</param>
        /// <param name="content">密文</param>
        /// <returns></returns>
        public static string Decrypt(RSACryptoServiceProvider rsa, params string[] content) => Decrypt(rsa, null,
            null, content);

        /// <summary>
        /// RSA解密
        /// </summary>
        /// <param name="privatekey">RSA私钥</param>
        /// <param name="encoding">字符编码</param>
        /// <param name="content">密文</param>
        /// <returns></returns>
        public static string Decrypt(string privatekey, Encoding encoding, params string[] content) => Decrypt(privatekey, encoding,
          null, content);

        /// <summary>
        /// RSA解密
        /// </summary>
        /// <param name="rsa">RSA私钥</param>
        /// <param name="encoding">字符编码</param>
        /// <param name="content">密文</param>
        /// <returns></returns>
        public static string Decrypt(RSACryptoServiceProvider rsa, Encoding encoding, params string[] content) => Decrypt(rsa, encoding,
            null, content);

        /// <summary>
        /// RSA解密
        /// </summary>
        /// <param name="privatekey">RSA私钥</param>
        /// <param name="encoding">字符编码</param>
        /// <param name="func">密文转Byte[]委托</param>
        /// <param name="content">密文</param>
        /// <returns>解密的明文</returns>
        public static string Decrypt(string privatekey, Encoding encoding = null,
            Func<string, byte[]> func = null, params string[] content) => Decrypt(GetRSACryptoServiceProvider(privatekey), encoding, func, content);

        /// <summary>
        /// RSA解密
        /// </summary>
        /// <param name="rsa">RSA私钥</param>
        /// <param name="encoding">字符编码（默认UTF8）</param>
        /// <param name="func">密文转Byte[]委托（默认encoding.GetBytes）</param>
        /// <param name="content">密文</param>
        /// <returns>解密的明文</returns>
        public static string Decrypt(RSACryptoServiceProvider rsa, Encoding encoding = null,
            Func<string, byte[]> func = null, params string[] content)
        {
            if (rsa == null)
                throw new ArgumentNullException(nameof(rsa));
            if (content == null || !content.Any())
                throw new ArgumentNullException(nameof(content));
            encoding = encoding ?? Encoding.UTF8;
            try
            {
                // ReSharper disable once AccessToDisposedClosure
                return string.Join(string.Empty,
                    content.Select(x => encoding.GetString(rsa.Decrypt(func != null ? func(x) : encoding.GetBytes(x), false))));
            }
            finally
            {
                rsa.Dispose();
            }
        }

        /// <summary>
        /// RSA加密
        /// </summary>
        /// <param name="privatekey">RSA私钥</param>
        /// <param name="value">明文</param>
        /// <param name="encoding">编码</param>
        /// <param name="fOAEP"></param>
        /// <param name="padding">填充模式</param>
        /// <returns></returns>
        public static string Encrypt(string privatekey, string value, Encoding encoding = null,
            // ReSharper disable once InconsistentNaming
            RSAEncryptionPadding padding = null, bool fOAEP = false) =>
            Encrypt(GetRSACryptoServiceProvider(privatekey), value, encoding, padding, fOAEP);

        /// <summary>
        /// RSA加密
        /// </summary>
        /// <param name="rsa">RSA私钥</param>
        /// <param name="value">明文</param>
        /// <param name="encoding">编码</param>
        /// <param name="fOAEP"></param>
        /// <param name="padding">填充模式</param>
        /// <returns></returns>
        // ReSharper disable once InconsistentNaming
        public static string Encrypt(RSACryptoServiceProvider rsa, string value, Encoding encoding = null, RSAEncryptionPadding padding = null, bool fOAEP = false)
        {
            encoding = encoding ?? Encoding.UTF8;
            byte[] valueBytes = encoding.GetBytes(value);
            byte[] encryptData = padding != null ? rsa.Encrypt(valueBytes, padding) : rsa.Encrypt(valueBytes, fOAEP);
            return encoding.GetString(encryptData);
        }

        // ReSharper disable once InconsistentNaming
        private static RSACryptoServiceProvider GetRSACryptoServiceProvider(string rsaKey)
        {
            if (string.IsNullOrWhiteSpace(rsaKey))
                throw new ArgumentNullException(nameof(rsaKey));
            var rsa = new RSACryptoServiceProvider();
            rsa.FromXmlStringByAigioSoft(rsaKey);
            return rsa;
        }
    }
}

namespace AigioSoft.Common
{
    // ReSharper disable once InconsistentNaming
    public static class RSAExtensions
    {
        /// <summary>
        /// RSACryptoServiceProvider.FromXmlString .NET Framework函数 标准库实现（参考反编译Framework函数代码）
        /// </summary>
        /// <param name="rsa"></param>
        /// <param name="xmlString"></param>
        public static void FromXmlStringByAigioSoft(this RSA rsa, string xmlString)
        {
            RSAParameters rsaParams = new RSAParameters();

            string modulusString = xmlString.SearchForTextOfLocalName("Modulus");
            if (modulusString == null)
                throw new CryptographicException("Cryptography_InvalidFromXmlString_RSA_Modulus");
            rsaParams.Modulus = Convert.FromBase64String(modulusString);

            string exponentString = xmlString.SearchForTextOfLocalName("Exponent");
            if (exponentString == null)
                throw new CryptographicException("Cryptography_InvalidFromXmlString_RSA_Exponent");
            rsaParams.Exponent = Convert.FromBase64String(exponentString);

            string pString = xmlString.SearchForTextOfLocalName("P");
            if (pString != null)
                rsaParams.P = Convert.FromBase64String(pString);

            string qString = xmlString.SearchForTextOfLocalName("Q");
            if (qString != null)
                rsaParams.Q = Convert.FromBase64String(qString);

            string dpString = xmlString.SearchForTextOfLocalName("DP");
            if (dpString != null)
                rsaParams.DP = Convert.FromBase64String(dpString);

            string dqString = xmlString.SearchForTextOfLocalName("DQ");
            if (dqString != null)
                rsaParams.DQ = Convert.FromBase64String(dqString);

            string inverseQString = xmlString.SearchForTextOfLocalName("InverseQ");
            if (inverseQString != null) rsaParams.InverseQ = Convert.FromBase64String(inverseQString);

            string dString = xmlString.SearchForTextOfLocalName("D");
            if (dString != null) rsaParams.D = Convert.FromBase64String(dString);

            rsa.ImportParameters(rsaParams);
        }

        private static string SearchForTextOfLocalName(this string str, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return null;
            var leftStr = $"<{name}>";
            var rightStr = $"</{name}>";
            var leftIndex = str.IndexOf(leftStr, StringComparison.OrdinalIgnoreCase);
            var rightIndex = str.IndexOf(rightStr, StringComparison.OrdinalIgnoreCase);
            return str.Substring(leftIndex + leftStr.Length, rightIndex - (leftIndex + leftStr.Length));
        }
    }
}
