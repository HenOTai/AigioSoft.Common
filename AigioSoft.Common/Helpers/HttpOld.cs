using System;
using System.Linq;
#if NET35 || NET40
using System.Net;
using System.IO;
using System.Text;
#endif
using m = AigioSoft.Common.Models;

namespace AigioSoft.Common.Helpers
{
    public static partial class Http
    {
#if NET35 || NET40

        private static WebResponse Send(m.HttpRequest request, out Version ver)
        {
            ver = request.Version;
            var requestUri = request.Method == m.HttpMethod.Get && request.Content.FormUrlEncodedContent != null ?
                request.RequestUri.AbsoluteUri + "?" + string.Join("&", request.Content.FormUrlEncodedContent.Select(x => $"{x.Key}={x.Value}").ToArray()) : request.RequestUri.AbsoluteUri;
            var webRequest = (HttpWebRequest)WebRequest.Create(requestUri);
            webRequest.ProtocolVersion = request.Version;
            if (request.Headers.CommonVersion != null)
            {
                var items = request.Headers.CommonVersion.Where(x => new[] { "User-Agent" }.Contains(x.Key, StringComparer.OrdinalIgnoreCase));
                var type = typeof(HttpRequestHeader);
                foreach (var item in items)
                {
                    var key = (HttpRequestHeader)Enum.Parse(type, item.Key);
                    if (Enum.IsDefined(type, key))
                        webRequest.Headers.Add(key, string.Join(";", item.Value.ToArray()));
                }
                var userAgent = request.Headers.CommonVersion.FindHeader("User-Agent")?.Value.FirstOrDefault();
                if (userAgent != null)
                    webRequest.UserAgent = userAgent;
            }
            if (request.Timeout.HasValue)
                webRequest.Timeout = request.Timeout.Value.Seconds;
            webRequest.Method = request.Method.Method;
            return webRequest.GetResponse();
        }

        private static Stream Send<T, T2>(m.HttpRequest request, out T response) where T : m.HttpResponse<T2>, new()
        {
            Version var;
            var webResponse = Send(request, out var);
            response = new T
            {
                Version = var,
                Headers = webResponse.Headers.AllKeys.ToDictionary(k => k, v => webResponse.Headers.GetValues(v)),
                IsSuccessStatusCode = true
            };
            return webResponse.GetResponseStream();
        }

        /// <summary>
        /// 发送Http请求（返回String类型正文）。
        /// </summary>
        /// <param name="request">请求参数。</param>
        /// <param name="encoding">返回正文编码（默认UTF8）。</param>
        /// <returns></returns>
        public static m.HttpResponseString SendAsString(m.HttpRequest request, Encoding encoding = null)
        {
            m.HttpResponseString response;
            var stream = Send<m.HttpResponseString, string>(request, out response);
            encoding = encoding ?? Encoding.UTF8;
            using (var reader = new StreamReader(stream, encoding))
                response.Content = reader.ReadToEnd();
            return response;
        }

        /// <summary>
        /// 发送Http请求（返回Byte[]类型正文）。
        /// </summary>
        /// <param name="request">请求参数。</param>
        /// <returns></returns>
        public static m.HttpResponseByteArray SendAsByteArray(m.HttpRequest request)
        {
            m.HttpResponseByteArray response;
            var stream = Send<m.HttpResponseByteArray, byte[]>(request, out response);
            var bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            stream.Close();
            return response;
        }

#endif
    }
}