using System;
using System.Collections.Generic;
using System.Linq;
#if !(NET35 || NET40)
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
#endif
using m = AigioSoft.Common.Models;

namespace AigioSoft.Common.Helpers
{
    /// <summary>
    /// Http请求操作
    /// </summary>
    public static partial class Http
    {
#if !(NET35 || NET40)

        /// <summary>
        /// 异步发送Http请求。
        /// </summary>
        /// <param name="httpClient">Http实例。</param>
        /// <param name="request">请求参数。</param>
        /// <returns></returns>
        public static async Task<HttpResponseMessage> SendAsync(this HttpClient httpClient, m.HttpRequest request)
        {
            #region Headers
            if (request.Headers.CommonVersion != null)
            {
                var items = request.Headers.CommonVersion.Where(x => new[] { "Accept", "Referrer", "User-Agent" }.Contains(x.Key, StringComparer.OrdinalIgnoreCase));
                foreach (var item in items)
                    httpClient.DefaultRequestHeaders.Add(item.Key, item.Value);
                var accept =
                    request.Headers.CommonVersion.FindHeader("Accept");
                if (accept != null)
                    foreach (var item in accept.Value.Value)
                        httpClient.DefaultRequestHeaders.Accept.ParseAdd(item);
                var referrer =
                    request.Headers.CommonVersion.FindHeader("Referrer")?.Value.FirstOrDefault();
                if (referrer != null)
                    httpClient.DefaultRequestHeaders.Referrer = new Uri(referrer);
                var userAgent =
                    request.Headers.CommonVersion.FindHeader("User-Agent")?.Value.FirstOrDefault();
                if (userAgent != null)
                    httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(userAgent);
            }
            else
                request.Headers.HightVersion?.Invoke(httpClient.DefaultRequestHeaders);
            #endregion

            if (request.Timeout.HasValue) httpClient.Timeout = request.Timeout.Value;
            if (request.MaxResponseContentBufferSize.HasValue) httpClient.MaxResponseContentBufferSize = request.MaxResponseContentBufferSize.Value;

            HttpResponseMessage response;
            if (request.Method == HttpMethod.Get)
                response = request.Content.FormUrlEncodedContent == null ?
                    await httpClient.GetAsync(request.RequestUri) :
                    await httpClient.GetAsync(new Uri(request.RequestUri.AbsoluteUri + "?" + string.Join("&", request.Content.FormUrlEncodedContent.Select(x => $"{x.Key}={x.Value}"))));
            else if (request.Method == HttpMethod.Post)
                response = await httpClient.PostAsync(request.RequestUri, request.Content.GetContent());
            else if (request.Method == HttpMethod.Put)
                response = await httpClient.PostAsync(request.RequestUri, request.Content.GetContent());
            else if (request.Method == HttpMethod.Delete)
                response = await httpClient.DeleteAsync(request.RequestUri);
            else
                throw new ArgumentOutOfRangeException(nameof(request.Method));
            return response;
        }

        /// <summary>
        /// 异步发送Http请求。
        /// </summary>
        /// <param name="request">请求参数。</param>
        /// <returns></returns>
        public static async Task<m.HttpResponseString> SendAsStringAsync(m.HttpRequest request)
        {
            using (var httpClient = new HttpClient())
            {
                var result = await httpClient.SendAsync(request);
                return await result.GetStringViewModel();
            }
        }

        /// <summary>
        /// 异步发送Http请求。
        /// </summary>
        /// <param name="requestAction">请求参数（委托传递）。</param>
        /// <returns></returns>
        public static async Task<m.HttpResponseString> SendAsStringAsync(Action<m.HttpRequest> requestAction)
        {
            var request = new m.HttpRequest();
            requestAction(request);
            return await SendAsStringAsync(request);
        }

        /// <summary>
        /// 发送Http请求（返回String类型正文）。
        /// </summary>
        /// <param name="request">请求参数。</param>
        /// <returns></returns>
        public static m.HttpResponseString SendAsString(m.HttpRequest request) => SendAsStringAsync(request).Result;

        /// <summary>
        /// 发送Http请求（返回String类型正文）。
        /// </summary>
        /// <param name="requestAction">请求参数（委托传递）。</param>
        /// <returns></returns>
        public static m.HttpResponseString SendAsString(Action<m.HttpRequest> requestAction) => SendAsStringAsync(requestAction).Result;

        /// <summary>
        /// 异步发送Http请求（返回Byte[]类型正文）。
        /// </summary>
        /// <param name="request">请求参数。</param>
        /// <returns></returns>
        public static async Task<m.HttpResponseByteArray> SendAsByteArrayAsync(m.HttpRequest request)
        {
            using (var httpClient = new HttpClient())
            {
                var result = await httpClient.SendAsync(request);
                return await result.GetByteArrayViewModel();
            }
        }

        /// <summary>
        /// 异步发送Http请求（返回Byte[]类型正文）。
        /// </summary>
        /// <param name="requestAction">请求参数（委托传递）。</param>
        /// <returns></returns>
        public static async Task<m.HttpResponseByteArray> SendAsByteArrayAsync(Action<m.HttpRequest> requestAction)
        {
            var request = new m.HttpRequest();
            requestAction(request);
            return await SendAsByteArrayAsync(request);
        }

        /// <summary>
        /// 发送Http请求（返回Byte[]类型正文）。
        /// </summary>
        /// <param name="request">请求参数。</param>
        /// <returns></returns>
        public static m.HttpResponseByteArray SendAsByteArray(m.HttpRequest request) => SendAsByteArrayAsync(request).Result;

        /// <summary>
        /// 发送Http请求（返回Byte[]类型正文）。
        /// </summary>
        /// <param name="requestAction">请求参数（委托传递）。</param>
        /// <returns></returns>
        public static m.HttpResponseByteArray SendAsByteArray(Action<m.HttpRequest> requestAction) => SendAsByteArrayAsync(requestAction).Result;
#endif
    }
}
