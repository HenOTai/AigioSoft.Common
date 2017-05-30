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
        /// 解密十六进制
        /// </summary>
        /// <param name="privatekey"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        // ReSharper disable once InconsistentNaming
        public static string DecryptHex(string privatekey, params string[] content)
        {
            if (content == null || !content.Any())
                return null;
            using (var rsa = new RSACryptoServiceProvider())
            {
                rsa.FromXmlString(privatekey);
                var result = content.Select(x => Encoding.UTF8.GetString(rsa.Decrypt(x.HexStringToBytes(), false)));
                return string.Join(string.Empty, result);
            }
        }

        /// <summary>
        /// 解密十六进制
        /// </summary>
        /// <param name="rsa"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        // ReSharper disable once InconsistentNaming
        public static string DecryptHex(RSACryptoServiceProvider rsa, params string[] content)
        {
            if (content == null || !content.Any())
                return null;
            var result = content.Select(x => Encoding.UTF8.GetString(rsa.Decrypt(x.HexStringToBytes(), false)));
            return string.Join(string.Empty, result);
        }
    }

}

namespace AigioSoft.Common
{
    // ReSharper disable once InconsistentNaming
    public static class RSAExtensions
    {
        public static void FromXmlString(this RSA rsa, string xmlString)
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
