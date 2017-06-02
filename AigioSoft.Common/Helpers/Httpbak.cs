
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
#if !(NET35 || NET40)
using System.Net.Http.Headers;
using System.Threading.Tasks;
#else
using System.Net;
#endif

namespace AigioSoft.Common.Helpers
{
    /// <summary>
    /// Http请求操作
    /// </summary>
    public static class Http
    {

#if !(NET35 || NET40)

        /// <summary>
        /// 以异步操作发送 HTTP 请求。
        /// </summary>
        /// <param name="httpClient">基本类，用于发送 HTTP 请求和接收来自通过 URI 确认的资源的 HTTP 响应。</param>
        /// <param name="url">请求地址。</param>
        /// <param name="method">请求方式（支持Get/Post/Put/Delete，默认Get）。</param>
        /// <param name="content">请求正文（仅在Post/Put方式中有效，一般使用System.Net.Http.StringContent）。</param>
        /// <param name="nameValueCollectionRequestHeaders">设置每个请求一起发送的标题。</param>
        /// <param name="referrer">设置 HTTP 请求的 Referer 标头值。</param>
        /// <param name="addAccepts">设置 HTTP 请求的 Accept 标头的值。</param>
        /// <param name="userAgent">设置 HTTP 请求的 User-Agent 标头值（默认AigioSoft.Common.Helpers.Http.UserAgent.Edge13_10586_Win10_x64）。</param>
        /// <param name="timeout">设置请求超时前等待的毫秒数。</param>
        /// <param name="maxResponseContentBufferSize">设置读取响应内容时要缓冲的最大字节数（默认值为 2 GB）。</param>
        /// <returns>表示包括状态代码和数据的 HTTP 响应消息。</returns>
        public static async Task<HttpResponseMessage> SendAsync(
            this System.Net.Http.HttpClient httpClient,
            string url,
            HttpMethod method = null,
            HttpContent content = null,
            Dictionary<string, string> nameValueCollectionRequestHeaders = null,
            string referrer = null,
            string userAgent = UserAgent.Edge13_10586_Win10_x64,
            TimeSpan? timeout = null,
            long? maxResponseContentBufferSize = null,
            IEnumerable<MediaTypeWithQualityHeaderValue> addAccepts = null
            )
        {
            return await httpClient.SendAsync(url, method, null, content, nameValueCollectionRequestHeaders, referrer, userAgent, timeout, maxResponseContentBufferSize, addAccepts);
        }

        /// <summary>
        /// 以异步操作发送 HTTP 请求。
        /// </summary>
        /// <param name="httpClient">基本类，用于发送 HTTP 请求和接收来自通过 URI 确认的资源的 HTTP 响应。</param>
        /// <param name="url">请求地址。</param>
        /// <param name="method">请求方式（支持Get/Post/Put/Delete，默认Get）。</param>
        /// <param name="nameValueCollectionContent">请求正文（键值对，仅在Get/Post/Put方式中有效，Get使用?/&amp;传参，Post/Put使用FormUrlEncodedContent）。</param>
        /// <param name="nameValueCollectionRequestHeaders">设置每个请求一起发送的标题。</param>
        /// <param name="referrer">设置 HTTP 请求的 Referer 标头值。</param>
        /// <param name="addAccepts">设置 HTTP 请求的 Accept 标头的值。</param>
        /// <param name="userAgent">设置 HTTP 请求的 User-Agent 标头值（默认AigioSoft.Common.Helpers.Http.UserAgent.Edge13_10586_Win10_x64）。</param>
        /// <param name="timeout">设置请求超时前等待的毫秒数。</param>
        /// <param name="maxResponseContentBufferSize">设置读取响应内容时要缓冲的最大字节数（默认值为 2 GB）。</param>
        /// <returns>表示包括状态代码和数据的 HTTP 响应消息。</returns>
        public static async Task<HttpResponseMessage> SendAsync(
            this System.Net.Http.HttpClient httpClient,
            string url,
            HttpMethod method = null,
            Dictionary<string, string> nameValueCollectionContent = null,
            Dictionary<string, string> nameValueCollectionRequestHeaders = null,
            string referrer = null,
            string userAgent = UserAgent.Edge13_10586_Win10_x64,
            TimeSpan? timeout = null,
            long? maxResponseContentBufferSize = null,
            IEnumerable<MediaTypeWithQualityHeaderValue> addAccepts = null
        )
        {
            return await httpClient.SendAsync(url, method, nameValueCollectionContent, null, nameValueCollectionRequestHeaders, referrer, userAgent, timeout, maxResponseContentBufferSize, addAccepts);
        }

        /// <summary>
        /// 以异步操作发送 HTTP 请求。
        /// </summary>
        /// <param name="httpClient">基本类，用于发送 HTTP 请求和接收来自通过 URI 确认的资源的 HTTP 响应。</param>
        /// <param name="url">请求地址。</param>
        /// <param name="method">请求方式（支持Get/Post/Put/Delete，默认Get）。</param>
        /// <param name="nameValueCollectionContent">请求正文（键值对，仅在Get/Post/Put方式中有效，Get使用?/&amp;传参，Post/Put使用FormUrlEncodedContent）。</param>
        /// <param name="content">请求正文（仅在Post/Put方式中有效，一般使用System.Net.Http.StringContent）。</param>
        /// <param name="nameValueCollectionRequestHeaders">设置每个请求一起发送的标题。</param>
        /// <param name="referrer">设置 HTTP 请求的 Referer 标头值。</param>
        /// <param name="addAccepts">设置 HTTP 请求的 Accept 标头的值。</param>
        /// <param name="userAgent">设置 HTTP 请求的 User-Agent 标头值（默认AigioSoft.Common.Helpers.Http.UserAgent.Edge13_10586_Win10_x64）。</param>
        /// <param name="timeout">设置请求超时前等待的毫秒数。</param>
        /// <param name="maxResponseContentBufferSize">设置读取响应内容时要缓冲的最大字节数（默认值为 2 GB）。</param>
        /// <returns>表示包括状态代码和数据的 HTTP 响应消息。</returns>
        internal static async Task<HttpResponseMessage> SendAsync(
            this System.Net.Http.HttpClient httpClient,
            string url,
            HttpMethod method = null,
            Dictionary<string, string> nameValueCollectionContent = null,
            HttpContent content = null,
            Dictionary<string, string> nameValueCollectionRequestHeaders = null,
            string referrer = null,
            string userAgent = UserAgent.Edge13_10586_Win10_x64,
            TimeSpan? timeout = null,
            long? maxResponseContentBufferSize = null,
            IEnumerable<MediaTypeWithQualityHeaderValue> addAccepts = null
        )
        {
            bool isGet = method == HttpMethod.Get;
            method = method ?? HttpMethod.Get;
            if (content == null && nameValueCollectionContent != null && !isGet)
                content = new FormUrlEncodedContent(nameValueCollectionContent);
            if (!string.IsNullOrWhiteSpace(userAgent))
                httpClient.DefaultRequestHeaders.Add("User-Agent", userAgent);
            if (maxResponseContentBufferSize.HasValue)
                httpClient.MaxResponseContentBufferSize = maxResponseContentBufferSize.Value;
            if (timeout.HasValue)
                httpClient.Timeout = timeout.Value;
            if (referrer != null)
                httpClient.DefaultRequestHeaders.Referrer = new Uri(referrer);
            if (addAccepts != null)
                foreach (var item in addAccepts)
                    httpClient.DefaultRequestHeaders.Accept.Add(item);
            nameValueCollectionRequestHeaders?.ToList()
                .ForEach(x => httpClient.DefaultRequestHeaders.Add(x.Key, x.Value));
            HttpResponseMessage response;
            if (isGet)
                response = nameValueCollectionContent == null ? await httpClient.GetAsync(new Uri(url)) : await httpClient.GetAsync(new Uri(url + "?" + string.Join("&", nameValueCollectionContent.Select(x => $"{x.Key}={x.Value}"))));
            else if (method == HttpMethod.Post)
                response = await httpClient.PostAsync(new Uri(url), content);
            else if (method == HttpMethod.Put)
                response = await httpClient.PostAsync(new Uri(url), content);
            else if (method == HttpMethod.Delete)
                response = await httpClient.DeleteAsync(new Uri(url));
            else
                throw new ArgumentOutOfRangeException(nameof(method));
            return response;
        }

        /// <summary>
        /// 以异步操作发送 HTTP 请求。
        /// </summary>
        /// <param name="url">请求地址。</param>
        /// <param name="method">请求方式（支持Get/Post/Put/Delete，默认Get）。</param>
        /// <param name="content">请求正文（仅在Post/Put方式中有效，一般使用System.Net.Http.StringContent）。</param>
        /// <param name="nameValueCollectionRequestHeaders">设置每个请求一起发送的标题。</param>
        /// <param name="referrer">设置 HTTP 请求的 Referer 标头值。</param>
        /// <param name="addAccepts">设置 HTTP 请求的 Accept 标头的值。</param>
        /// <param name="userAgent">设置 HTTP 请求的 User-Agent 标头值（默认AigioSoft.Common.Helpers.Http.UserAgent.Edge13_10586_Win10_x64）。</param>
        /// <param name="timeout">设置请求超时前等待的毫秒数。</param>
        /// <param name="maxResponseContentBufferSize">设置读取响应内容时要缓冲的最大字节数（默认值为 2 GB）。</param>
        /// <returns>表示包括状态代码和数据的 HTTP 响应消息。</returns>
        public static async Task<HttpResponseByteArrayModel> SendReadAsByteArrayAsync(
            string url,
            HttpMethod method = null,
            HttpContent content = null,
            Dictionary<string, string> nameValueCollectionRequestHeaders = null,
            string referrer = null,
            string userAgent = UserAgent.Edge13_10586_Win10_x64,
            TimeSpan? timeout = null,
            long? maxResponseContentBufferSize = null,
            IEnumerable<MediaTypeWithQualityHeaderValue> addAccepts = null
        )
        {
            using (var httpClient = new System.Net.Http.HttpClient())
            {
                var response = await httpClient.SendAsync(url, method, null, content, nameValueCollectionRequestHeaders, referrer, userAgent,
                    timeout, maxResponseContentBufferSize, addAccepts);
                return await response.GetViewByteArrayModel();
            }
        }

        /// <summary>
        /// 以异步操作发送 HTTP 请求。
        /// </summary>
        /// <param name="url">请求地址。</param>
        /// <param name="method">请求方式（支持Get/Post/Put/Delete，默认Get）。</param>
        /// <param name="content">请求正文（仅在Post/Put方式中有效，一般使用System.Net.Http.StringContent）。</param>
        /// <param name="nameValueCollectionRequestHeaders">设置每个请求一起发送的标题。</param>
        /// <param name="referrer">设置 HTTP 请求的 Referer 标头值。</param>
        /// <param name="addAccepts">设置 HTTP 请求的 Accept 标头的值。</param>
        /// <param name="userAgent">设置 HTTP 请求的 User-Agent 标头值（默认AigioSoft.Common.Helpers.Http.UserAgent.Edge13_10586_Win10_x64）。</param>
        /// <param name="timeout">设置请求超时前等待的毫秒数。</param>
        /// <param name="maxResponseContentBufferSize">设置读取响应内容时要缓冲的最大字节数（默认值为 2 GB）。</param>
        /// <returns>表示包括状态代码和数据的 HTTP 响应消息。</returns>
        public static async Task<HttpResponseStringModel> SendReadAsStringAsync(
            string url,
            HttpMethod method = null,
            HttpContent content = null,
            Dictionary<string, string> nameValueCollectionRequestHeaders = null,
            string referrer = null,
            string userAgent = UserAgent.Edge13_10586_Win10_x64,
            TimeSpan? timeout = null,
            long? maxResponseContentBufferSize = null,
            IEnumerable<MediaTypeWithQualityHeaderValue> addAccepts = null
        )
        {
            using (var httpClient = new System.Net.Http.HttpClient())
            {
                var response = await httpClient.SendAsync(url, method, null, content, nameValueCollectionRequestHeaders, referrer, userAgent,
                    timeout, maxResponseContentBufferSize, addAccepts);
                return await response.GetViewStringModel();
            }
        }

        /// <summary>
        /// 以异步操作发送 HTTP 请求。
        /// </summary>
        /// <param name="url">请求地址。</param>
        /// <param name="method">请求方式（支持Get/Post/Put/Delete，默认Get）。</param>        /// <param name="nameValueCollectionContent">请求正文（键值对，仅在Get/Post/Put方式中有效，Get使用?/&amp;传参，Post/Put使用FormUrlEncodedContent）。</param>
        /// <param name="nameValueCollectionRequestHeaders">设置每个请求一起发送的标题。</param>
        /// <param name="referrer">设置 HTTP 请求的 Referer 标头值。</param>
        /// <param name="addAccepts">设置 HTTP 请求的 Accept 标头的值。</param>
        /// <param name="userAgent">设置 HTTP 请求的 User-Agent 标头值（默认AigioSoft.Common.Helpers.Http.UserAgent.Edge13_10586_Win10_x64）。</param>
        /// <param name="timeout">设置请求超时前等待的毫秒数。</param>
        /// <param name="maxResponseContentBufferSize">设置读取响应内容时要缓冲的最大字节数（默认值为 2 GB）。</param>
        /// <returns>表示包括状态代码和数据的 HTTP 响应消息。</returns>
        public static async Task<HttpResponseByteArrayModel> SendReadAsByteArrayAsync(
            string url,
            HttpMethod method = null,
            Dictionary<string, string> nameValueCollectionContent = null,
            Dictionary<string, string> nameValueCollectionRequestHeaders = null,
            string referrer = null,
            string userAgent = UserAgent.Edge13_10586_Win10_x64,
            TimeSpan? timeout = null,
            long? maxResponseContentBufferSize = null,
            IEnumerable<MediaTypeWithQualityHeaderValue> addAccepts = null
        )
        {
            using (var httpClient = new System.Net.Http.HttpClient())
            {
                var response = await httpClient.SendAsync(url, method, nameValueCollectionContent, null, nameValueCollectionRequestHeaders, referrer, userAgent,
                    timeout, maxResponseContentBufferSize, addAccepts);
                return await response.GetViewByteArrayModel();
            }
        }

        /// <summary>
        /// 以异步操作发送 HTTP 请求。
        /// </summary>
        /// <param name="url">请求地址。</param>
        /// <param name="method">请求方式（支持Get/Post/Put/Delete，默认Get）。</param>        /// <param name="nameValueCollectionContent">请求正文（键值对，仅在Get/Post/Put方式中有效，Get使用?/&amp;传参，Post/Put使用FormUrlEncodedContent）。</param>
        /// <param name="nameValueCollectionRequestHeaders">设置每个请求一起发送的标题。</param>
        /// <param name="referrer">设置 HTTP 请求的 Referer 标头值。</param>
        /// <param name="addAccepts">设置 HTTP 请求的 Accept 标头的值。</param>
        /// <param name="userAgent">设置 HTTP 请求的 User-Agent 标头值（默认AigioSoft.Common.Helpers.Http.UserAgent.Edge13_10586_Win10_x64）。</param>
        /// <param name="timeout">设置请求超时前等待的毫秒数。</param>
        /// <param name="maxResponseContentBufferSize">设置读取响应内容时要缓冲的最大字节数（默认值为 2 GB）。</param>
        /// <returns>表示包括状态代码和数据的 HTTP 响应消息。</returns>
        public static async Task<HttpResponseStringModel> SendReadAsStringAsync(
            string url,
            HttpMethod method = null,
            Dictionary<string, string> nameValueCollectionContent = null,
            Dictionary<string, string> nameValueCollectionRequestHeaders = null,
            string referrer = null,
            string userAgent = UserAgent.Edge13_10586_Win10_x64,
            TimeSpan? timeout = null,
            long? maxResponseContentBufferSize = null,
            IEnumerable<MediaTypeWithQualityHeaderValue> addAccepts = null
        )
        {
            using (var httpClient = new System.Net.Http.HttpClient())
            {
                var response = await httpClient.SendAsync(url, method, nameValueCollectionContent, null, nameValueCollectionRequestHeaders, referrer, userAgent,
                    timeout, maxResponseContentBufferSize, addAccepts);
                return await response.GetViewStringModel();
            }
        }

#else

        public static HttpResponseByteArrayModel SendReadAsByteArray(
            string url,
            HttpMethod method = null,
            Dictionary<string, string> nameValueCollectionContent = null,
            Dictionary<string, string> nameValueCollectionRequestHeaders = null,
            string referrer = null,
            string userAgent = UserAgent.Edge13_10586_Win10_x64,
            TimeSpan? timeout = null,
            long? maxResponseContentBufferSize = null
            )
        {
            method = method ?? HttpMethod.Get;
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = method.Method;
            request.UserAgent = userAgent;
            return null;
        }

        public static HttpResponseStringModel SendReadAsString()
        {
            return null;
        }

#endif
    }
}