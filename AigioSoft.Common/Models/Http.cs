using AigioSoft.Common.Models;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#if !(NET20 || NET35 || NET40)
using System.Net.Http;
using System.Threading.Tasks;
#endif

// ReSharper disable once CheckNamespace
namespace AigioSoft.Common.Models
{
    #region HttpRequest

    /// <summary>
    /// 表示 HTTP 请求模型。
    /// </summary>
    public class HttpRequest
    {
        /// <summary>
        /// 初始化 AigioSoft.Common.HttpRequest  类的新实例。
        /// </summary>
        public HttpRequest()
        {
        }

        /// <summary>
        /// 初始化 HTTP 请求 System.Uri 的 AigioSoft.Common.HttpRequest  类的新实例。
        /// </summary>
        /// <param name="requestUri"></param>
        public HttpRequest(string requestUri)
        {
            RequestUri = new Uri(requestUri);
        }

        /// <summary>
        /// 初始化 HTTP 方法和请求 System.Uri 的 AigioSoft.Common.HttpRequest  类的新实例。
        /// </summary>
        /// <param name="method"></param>
        /// <param name="requestUri"></param>
        public HttpRequest(HttpMethod method, Uri requestUri)
        {
            Method = method;
            RequestUri = requestUri;
        }

        /// <summary>
        /// 初始化 HTTP 方法和请求 System.Uri 的 AigioSoft.Common.HttpRequest  类的新实例。
        /// </summary>
        /// <param name="method"></param>
        /// <param name="requestUri"></param>
        public HttpRequest(HttpMethod method, string requestUri)
        {
            Method = method;
            RequestUri = new Uri(requestUri);
        }

        /// <summary>
        /// 获取或设置 HTTP 消息版本（ 默认值为 1.1）。
        /// </summary>
        public Version Version { get; set; } = new Version(1, 1);

        /// <summary>
        /// 获取或设置 HTTP 消息的内容。
        /// </summary>
        public HttpContent Content { get; set; } = null;

        /// <summary>
        /// 获取或设置 HTTP 请求信息使用的 HTTP 方法（默认为Get）。
        /// </summary>
        public HttpMethod Method { get; set; } = HttpMethod.Get;

        /// <summary>
        /// 获取或设置 HTTP 请求的 System.Uri。
        /// </summary>
        public Uri RequestUri { get; set; }

        /// <summary>
        ///  获取或设置 HTTP 请求标头的集合。
        /// </summary>
        public HttpRequestHeadersType Headers { get; set; }

        /// <summary>
        /// 获取或设置请求超时前等待的时间跨度。
        /// </summary>
        public TimeSpan? Timeout { get; set; }

        /// <summary>
        /// 获取或设置读取响应内容时要缓冲的最大字节数。
        /// </summary>
        public long? MaxResponseContentBufferSize { get; set; }
    }

    #endregion HttpRequest

    #region HttpRequestHeaders

    /// <summary>
    /// 表示“请求标题”的集合（HightVersion使用System.Net.Http）。
    /// </summary>
    public sealed class HttpRequestHeadersType
    {
        internal HttpRequestHeadersType()
        {
        }

        public HttpRequestHeaders CommonVersion { get; set; }

        public HttpRequestHeadersType Create(HttpRequestHeaders headers) => headers == null
            ? null
            : new HttpRequestHeadersType
            {
                CommonVersion = headers
            };

#if !(NET20 || NET35 || NET40)
        public Action<System.Net.Http.Headers.HttpRequestHeaders> HightVersion { get; set; }

        public HttpRequestHeadersType Create(Action<System.Net.Http.Headers.HttpRequestHeaders> headers) => headers == null
            ? null
            : new HttpRequestHeadersType
            {
                HightVersion = headers
            };

#endif
    }

    /// <summary>
    /// 表示“请求标题”的集合。
    /// </summary>
    [JsonObject]
    public sealed class HttpRequestHeaders : IEnumerable<KeyValuePair<string, IEnumerable<string>>>
    {
        internal HttpRequestHeaders()
        {
        }

        public static HttpRequestHeaders Create(Dictionary<string, IEnumerable<string>> headers) => headers == null
            ? null
            : new HttpRequestHeaders { Headers = headers };

        private Dictionary<string, IEnumerable<string>> _headers;

        public Dictionary<string, IEnumerable<string>> Headers
        {
            get
            {
                if (!_headers.Keys.Any(x => x.Equals("Accept", StringComparison.OrdinalIgnoreCase)) && (Accept?.Any() ?? false))
                    _headers.Add("Accept", Accept.ToArray());
                if (Referrer != null)
                    _headers.Add("Referrer", new[] { Referrer });
                if (Referrer != null)
                    _headers.Add("User-Agent", new[] { UserAgent });
                return _headers;
            }
            private set => _headers = value;
        }

        public IEnumerable<string> this[string key] => Headers[key];

        public Dictionary<string, IEnumerable<string>>.ValueCollection Values => Headers.Values;

        public Dictionary<string, IEnumerable<string>>.KeyCollection Keys => Headers.Keys;

        public KeyValuePair<string, IEnumerable<string>>? FindHeader(string key)
        {
            if (_headers.Keys.Any(x => x.Equals(key, StringComparison.OrdinalIgnoreCase)))
                return _headers.First(
                    x => x.Key.Equals(key, StringComparison.OrdinalIgnoreCase));
            return null;
        }

        /// <summary>
        /// 添加指定的标头及其值到 System.Net.Http.Headers.HttpHeaders 集合中。
        /// </summary>
        /// <param name="name">要添加到集合中的标头。</param>
        /// <param name="value">标头的内容。</param>
        public void Add(string name, string value) => Headers.Add(name, new[] { value });

        public void Add(string name, string[] value) => Headers.Add(name, value);

        public void Clear() => Headers.Clear();

        public bool Contains(string key) => Headers.ContainsKey(key);

        IEnumerator<KeyValuePair<string, IEnumerable<string>>> IEnumerable<KeyValuePair<string, IEnumerable<string>>>.GetEnumerator() => Headers.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => Headers.GetEnumerator();

        /// <summary>
        /// 获取或设置 HTTP 请求的 Accept 标头的值。
        /// </summary>
        public List<string> Accept { get; set; } = new List<string>(0);

        public string Referrer { get; set; }

        public string UserAgent { get; set; } = Helpers.Http.UserAgent.Edge13_10586_Win10_x64;

        ///// <summary>
        ///// 获取或设置 HTTP 请求的 Upgrade 标头的值。
        ///// </summary>
        //public List<string> Upgrade { get; set; } = new List<string>(0);

        ///// <summary>
        ///// 获取或设置 HTTP 请求的 Transfer-Encoding 标头的值。
        ///// </summary>
        //public List<string> TransferEncoding { get; set; } = new List<string>(0);

        ///// <summary>
        ///// 获取或设置 HTTP 请求的 Trailer 标头的值。
        ///// </summary>
        //public List<string> Trailer { get; set; } = new List<string>(0);

        ///// <summary>
        ///// 获取或设置 HTTP 请求的 Pragma 标头的值。
        ///// </summary>
        //public List<string> Pragma { get; set; } = new List<string>(0);

        ///// <summary>
        ///// 获取或设置 HTTP 请求的 Date 标头值。
        ///// </summary>
        //public DateTimeOffset? Date { get; set; }
    }

    #endregion HttpRequestHeaders

    #region HttpRequestContent

    /// <summary>
    /// HTTP 消息的内容模型。
    /// </summary>
    public class HttpContent
    {
        internal HttpContent()
        {
        }

        /// <summary>
        /// 创建 HTTP 消息的内容（纯文本）。
        /// </summary>
        /// <param name="content"></param>
        /// <param name="encoding"></param>
        /// <param name="mediaType"></param>
        /// <returns></returns>
        public static HttpContent Create(string content, Encoding encoding = null, string mediaType = null)
        {
            if (content == null)
                return null;
            var result = new HttpContent
            {
                Content = content
            };
            if (encoding != null)
                result.Encoding = encoding;
            if (mediaType != null)
                result.MediaType = mediaType;
            return result;
        }

        /// <summary>
        /// 创建 HTTP 消息的内容（键/值对集合）。
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static HttpContent Create(Dictionary<string, string> content) => content == null ? null : new HttpContent { FormUrlEncodedContent = content };

#if !(NET20 || NET35 || NET40)
        internal System.Net.Http.HttpContent GetContent()
        {
            if (FormUrlEncodedContent != null)
                return new FormUrlEncodedContent(FormUrlEncodedContent);
            if (Content != null)
                return new StringContent(Content, Encoding, MediaType);
            return null;
        }
#endif

        /// <summary>
        /// 获取或设置 HTTP 消息的内容（纯文本）。
        /// </summary>
        internal string Content { get; set; } = null;

        internal Encoding Encoding { get; set; } = Encoding.UTF8;

        internal string MediaType { get; set; } = "text/plain";

        /// <summary>
        /// 获取或设置 HTTP 消息的内容（键/值对集合）。
        /// </summary>
        internal Dictionary<string, string> FormUrlEncodedContent { get; set; } = null;
    }

    #endregion HttpRequestContent

    #region HttpResponse

    /// <summary>
    /// HTTP 响应模型基类。
    /// </summary>
    public abstract class BaseHttpResponse
    {
        /// <summary>
        /// 获取或设置 HTTP 消息版本（ 默认值为 1.1）。
        /// </summary>
        public Version Version { get; set; } = new Version(1, 1);

        /// <summary>
        /// 获取或设置 HTTP 响应标头的集合。
        /// </summary>
        public Dictionary<string, string[]> Headers { get; set; }

        /// <summary>
        /// 获取或设置一个值，该值指示 HTTP 响应是否成功（ 一个指示 HTTP 响应是否成功的值。 如果 System.Net.Http.HttpResponseMessage.StatusCode 在 200-299范围内，则为 true；否则为 false）。
        /// </summary>
        public bool IsSuccessStatusCode { get; set; }
    }

    /// <summary>
    /// 表示 HTTP 响应模型（正文类型为T）。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class HttpResponse<T> : BaseHttpResponse
    {
        /// <summary>
        /// 获取或设置 HTTP 响应消息的内容。
        /// </summary>
        public T Content { get; set; }
    }

    /// <summary>
    /// 表示 HTTP 响应模型（正文类型为System.Byte[]）。
    /// </summary>
    public class HttpResponseByteArray : HttpResponse<byte[]>
    {
    }

    /// <summary>
    /// 表示 HTTP 响应模型（正文类型为System.String）。
    /// </summary>
    public class HttpResponseString : HttpResponse<string>
    {
    }

    #endregion HttpResponse

#if (NET20 || NET35 || NET40)

    /// <summary>
    /// 一个帮助器类，它用于检索并比较标准 HTTP 方法并且用于创建新的 HTTP 方法。
    /// </summary>
    public class HttpMethod
    {
        /// <summary>
        /// 表示一个 HTTP GET 协议方法。
        /// </summary>
        public static HttpMethod Get => new HttpMethod("Get");

        /// <summary>
        /// 表示一个 HTTP PUT 协议方法，该方法用于替换 URI 标识的实体。
        /// </summary>
        public static HttpMethod Put => new HttpMethod("Put");

        /// <summary>
        /// 表示一个 HTTP POST 协议方法，该方法用于将新实体作为补充发送到某个 URI。
        /// </summary>
        public static HttpMethod Post => new HttpMethod("Post");

        /// <summary>
        /// 表示一个 HTTP DELETE 协议方法。
        /// </summary>
        public static HttpMethod Delete => new HttpMethod("Delete");

        /// <summary>
        /// 表示一个 HTTP HEAD 协议方法。 除了服务器在响应中只返回消息头不返回消息体以外，HEAD 方法和 GET 是一样的。
        /// </summary>
        public static HttpMethod Head => new HttpMethod("Head");

        /// <summary>
        /// 表示一个 HTTP OPTIONS 协议方法。
        /// </summary>
        public static HttpMethod Options => new HttpMethod("Options");

        /// <summary>
        /// 表示一个 HTTP TRACE 协议方法。
        /// </summary>
        public static HttpMethod Trace => new HttpMethod("Trace");

        /// <summary>
        /// HTTP 方法。
        /// </summary>
        public string Method { get; }

        /// <summary>
        /// 确定指定的 System.Net.Http.HttpMethod 是否等于当前的 System.Object。
        /// </summary>
        /// <param name="other">要与当前目标进行比较的 HTTP 方法。</param>
        /// <returns>返回 System.Boolean。 如果指定的对象等于当前对象，则为 true；否则为 false。</returns>
        public bool Equals(HttpMethod other) => Equals(other.Method);

        /// <summary>
        /// 确定指定的 System.Net.Http.HttpMethod 是否等于当前的 System.String。
        /// </summary>
        /// <param name="other">要与当前目标进行比较的字符串。</param>
        /// <returns>返回 System.Boolean。 如果指定的对象等于当前对象，则为 true；否则为 false。</returns>
        public bool Equals(string other) => string.Equals(Method, other, StringComparison.OrdinalIgnoreCase);

        /// <summary>
        /// 确定指定的 System.Object 是否等于当前的 System.Object。
        /// </summary>
        /// <param name="obj">要与当前对象进行比较的对象。</param>
        /// <returns>返回 System.Boolean。 如果指定的对象等于当前对象，则为 true；否则为 false。</returns>
        public override bool Equals(object obj) => obj != null && GetHashCode() == obj.GetHashCode();

        /// <summary>
        /// 用作此类型的哈希函数。
        /// </summary>
        /// <returns>返回 System.Int32。 当前 System.Object 的哈希代码。</returns>
        public override int GetHashCode() => Method.ToLower().GetHashCode();

        /// <summary>
        /// 返回表示当前对象的字符串。
        /// </summary>
        /// <returns>返回 System.String。 一个表示当前对象的字符串。</returns>
        public override string ToString() => Method;

        /// <summary>
        /// 用于比较两个 System.Net.Http.HttpMethod 对象的相等运算符。
        /// </summary>
        /// <param name="left">相等运算符左侧的 System.Net.Http.HttpMethod。</param>
        /// <param name="right">相等运算符右侧的 System.Net.Http.HttpMethod。</param>
        /// <returns>返回 System.Boolean。 如果指定的 left 和 right 参数相等，则为 true；否则为 false。</returns>
        public static bool operator ==(HttpMethod left, HttpMethod right) => left?.Equals(right) ?? false;

        /// <summary>
        /// 用于比较两个 System.Net.Http.HttpMethod 对象的不相等运算符。
        /// </summary>
        /// <param name="left">不相等运算符左侧的 System.Net.Http.HttpMethod。</param>
        /// <param name="right">不相等运算符右侧的 System.Net.Http.HttpMethod。</param>
        /// <returns>返回 System.Boolean。 如果指定的 left 和 right 参数不相等，则为 true；否则为 false。</returns>
        public static bool operator !=(HttpMethod left, HttpMethod right) => !left?.Equals(right) ?? true;

        public HttpMethod(string method)
        {
            Method = method;
        }
    }

#endif
}

#if !(NET20 || NET35 || NET40)
namespace AigioSoft.Common.Helpers
{
    /// <summary>
    /// HTTP 响应消息扩展
    /// </summary>
    public static class HttpResponseMessageExtension
    {
        internal static T GetViewModel<T, T2>(this HttpResponseMessage model) where T : HttpResponse<T2>, new()
        {
            return new T
            {
                IsSuccessStatusCode = model.IsSuccessStatusCode,
                Headers = model.Headers.ToDictionary(x => x.Key, y => y.Value.ToArray()),
                Version = model.Version
            };
        }

        /// <summary>
        /// 获取响应消息模型（ByteArray）。
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static async Task<HttpResponseByteArray> GetByteArrayViewModel(this HttpResponseMessage model)
        {
            HttpResponseByteArray result = model.GetViewModel<HttpResponseByteArray, byte[]>();
            result.Content = await model.Content.ReadAsByteArrayAsync();
            return result;
        }

        /// <summary>
        /// 获取响应消息模型（String）。
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static async Task<HttpResponseString> GetStringViewModel(this HttpResponseMessage model)
        {
            HttpResponseString result = model.GetViewModel<HttpResponseString, string>();
            result.Content = await model.Content.ReadAsStringAsync();
            return result;
        }
    }
}
#endif