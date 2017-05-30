using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

// ReSharper disable once CheckNamespace
namespace AigioSoft.Common.Helpers
{
    /// <summary>
    /// 功能-登陆
    /// </summary>
    public static class Login
    {
        /// <summary>
        /// 解密登陆密文
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ciphertext"></param>
        /// <param name="rsa"></param>
        /// <returns></returns>
        private static T Decryption<T>(string ciphertext, RSACryptoServiceProvider rsa) where T : LoginDecryptionModel
        {
            return JsonConvert.DeserializeObject<T>(RSA.DecryptHex(rsa, ciphertext
                .Split((new[] { ' ' }).Concat(Environment.NewLine.ToArray()).ToArray(),
                    StringSplitOptions.RemoveEmptyEntries)));
        }

        /// <summary>
        /// 获取按参数名排序的值数组Json字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        private static string GetValidationString<T>(T model) where T : LoginDecryptionMainModel
        {
            if (model == null)
                return null;
            var type = model.GetType();
            var linq = from m in type.GetProperties()
                       let attr = m.GetCustomAttribute<JsonPropertyAttribute>()
                       where !string.IsNullOrWhiteSpace(attr?.PropertyName)
                       orderby attr.PropertyName
                       select m.GetValue(model);
            return JsonConvert.SerializeObject(linq.ToArray(), Formatting.None);
        }

        /// <summary>
        /// 验证哈希算法
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private static LoginDecryptionValidationType Validation(LoginDecryptionModel model)
        {
            var mainJsonStr = GetValidationString(model.Main);
            var salt = $"{DateTime.Now.Year}.{DateTime.Now.Month}";
            if (Hashs.MD5($"as_ygk[{mainJsonStr}]ygk_md5_OnlyUsedAuthenticate_{salt}") != model.Hash.Md5)
                return LoginDecryptionValidationType.HashValidationFails;
            if (Hashs.SHA1($"as_ygk[{mainJsonStr}]ygk_sha1_OnlyUsedAuthenticate_{salt}") != model.Hash.Sha1)
                return LoginDecryptionValidationType.HashValidationFails;
            if (Hashs.SHA512($"as_ygk[{mainJsonStr}]ygk_sha512_OnlyUsedAuthenticate_{salt}") != model.Hash.Sha512)
                return LoginDecryptionValidationType.HashValidationFails;
            return LoginDecryptionValidationType.Success;
        }

        /// <summary>
        /// 主处理函数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ciphertext"></param>
        /// <param name="rsaprivatekey"></param>
        /// <param name="logger"></param>
        /// <returns></returns>
        public static (LoginDecryptionValidationType type, LoginDecryptionModel result) Main<T>(string ciphertext,
            string rsaprivatekey, ILogger logger = null) where T : LoginDecryptionModel
        {
            try
            {
                using (var rsa = new RSACryptoServiceProvider())
                {
                    rsa.FromXmlString(rsaprivatekey);
                    var deserialize = Decryption<T>(ciphertext, rsa);
                    if (deserialize?.Main == null || deserialize.Hash == null || !deserialize.Main.CreatTime.HasValue)
                        return (LoginDecryptionValidationType.InvalidFormat, null); //格式有误
                    if (!(deserialize.Main.CreatTime.Value.Year == DateTime.Today.Year
                          && deserialize.Main.CreatTime.Value.Month == DateTime.Today.Month))
                        return (LoginDecryptionValidationType.TimeTooLong, null); //不在当月 已知问题：一个月最后一天跨零点提交 
                    var validationType = Validation(deserialize);
                    if (validationType != LoginDecryptionValidationType.Success)
                        return (validationType, null); //哈希验证失败
                    deserialize.Main.Password = WebUtility.UrlDecode(Common.Helpers.RSA.DecryptHex(rsa,
                        deserialize.Main.Password.Split((new[] { ' ' }).Concat(Environment.NewLine.ToArray()).ToArray(),
                            StringSplitOptions.RemoveEmptyEntries)));
                    deserialize.Main.UserName = WebUtility.UrlDecode(deserialize.Main.UserName);
                    return (LoginDecryptionValidationType.Success, deserialize); //通过
                }
            }
            catch (Exception ex)
            {
                logger.LogError(5000, $"LoginHelper_Main：{ex.Message}");
                return (LoginDecryptionValidationType.Exception, null); //出现异常
            }
        }
    }
}
namespace AigioSoft.Common
{

    public enum LoginDecryptionValidationType : byte
    {
        /// <summary>
        /// 成功
        /// </summary>
        [Description("成功")]
        Success = 0,

        /// <summary>
        /// 格式错误
        /// </summary>
        [Description("格式错误")]
        InvalidFormat = 200,

        /// <summary>
        /// 时间失效
        /// </summary>
        [Description("时间失效")]
        TimeTooLong = 201,

        /// <summary>
        /// 哈希算法验证失败
        /// </summary>
        [Description("哈希算法验证失败")]
        HashValidationFails = 202,

        /// <summary>
        /// 发生异常
        /// </summary>
        [Description("发生异常")]
        Exception = 255
    }


    public class LoginDecryptionModel
    {
        [JsonProperty("main")]
        public LoginDecryptionMainModel Main { get; set; }

        [JsonProperty("hash")]
        public LoginDecryptionHashModel Hash { get; set; }
    }

    public class LoginDecryptionMainModel
    {
        [JsonProperty("mima")]
        public string Password { get; set; }

        [JsonProperty("securitycode")]
        public string SecurityCode { get; set; }

        [JsonProperty("username")]
        public string UserName { get; set; }

        [JsonProperty("time")]
        public string Time { get; set; }

        [JsonIgnore]
        public DateTime? CreatTime
        {
            get
            {
                if (System.DateTime.TryParseExact(Time, "yyyy-MM-ddTHH:mm:ss.fffZ", CultureInfo.CurrentCulture,
                    DateTimeStyles.None, out DateTime dt))
                    return dt;
                return null;
            }
        }
    }

    public class LoginDecryptionHashModel
    {
        [JsonProperty("md5")]
        public string Md5 { get; set; }

        [JsonProperty("sha1")]
        public string Sha1 { get; set; }

        [JsonProperty("sha512")]
        public string Sha512 { get; set; }
    }

    public enum LoginType : byte
    {
        登陆成功 = 0,

        无效的用户名 = 100,
        无效的密码 = 101,
        用户名或密码无效 = 102,
        无效的登录尝试 = 103,
        无效的验证码 = 104,
        验证码错误 = 105,

        需要双重验证身份 = 201,
        账户锁定 = 202,

        解密失败 = 251,

        验证码超时 = 107,
        // ReSharper disable once InconsistentNaming
        未知的IP地址 = 109,
        记录保存失败 = 111
    }

    public class LoginResult
    {
        public LoginResult(LoginType code, string customMessage = null, bool securityCode = false)
        {
            Code = code;
            Message = "登陆" + (code == LoginType.登陆成功 ? "成功" : "失败") + "，" + (customMessage ?? Code.ToString()) + "。";
            SecurityCode = securityCode;
        }

        public LoginType Code { get; set; }
        public string Message { get; set; }

        public bool SecurityCode { get; set; }
    }
}
