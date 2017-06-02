//#if !(NET20 || NET35 || NET40)

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net.Http;
//using System.Net.Http.Headers;
//using System.Threading.Tasks;

//namespace AigioSoft.Common.Helpers
//{
//    /// <summary>
//    /// Http请求操作
//    /// </summary>
//    public static class HttpClient
//    {
//        /// <summary>
//        /// 发送Http请求
//        /// </summary>
//        /// <param name="url"></param>
//        /// <param name="method"></param>
//        /// <param name="nameValueCollection"></param>
//        /// <param name="maxResponseContentBufferSize"></param>
//        /// <param name="timeout"></param>
//        /// <param name="userAgent"></param>
//        /// <param name="referrer"></param>
//        /// <param name="addAccepts"></param>
//        /// <returns></returns>
//        [Obsolete("AigioSoft.Common.Helpers.Http.SendReadAsStringAsync", true)]
//        public static string Send(string url, string method = "GET",
//            IEnumerable<KeyValuePair<string, string>> nameValueCollection = null,
//            long? maxResponseContentBufferSize = null,
//            TimeSpan? timeout = null, string userAgent = null, Uri referrer = null,
//            IEnumerable<MediaTypeWithQualityHeaderValue> addAccepts = null)
//        {
//            throw new NotSupportedException();
//            using (var httpClient = new System.Net.Http.HttpClient())
//            {
//                if (!string.IsNullOrWhiteSpace(userAgent))
//                    httpClient.DefaultRequestHeaders.Add("User-Agent", userAgent);
//                if (maxResponseContentBufferSize.HasValue)
//                    httpClient.MaxResponseContentBufferSize = maxResponseContentBufferSize.Value;
//                if (timeout.HasValue)
//                    httpClient.Timeout = timeout.Value;
//                if (referrer != null)
//                    httpClient.DefaultRequestHeaders.Referrer = referrer;
//                if (addAccepts != null)
//                    foreach (var item in addAccepts)
//                        httpClient.DefaultRequestHeaders.Accept.Add(item);
//                Task<HttpResponseMessage> response;
//                if (string.Equals(method, "GET", StringComparison.OrdinalIgnoreCase))
//                    response = httpClient.GetAsync(new Uri(url + "?" + string.Join("&", nameValueCollection.Select(x => $"{x.Key}={x.Value}"))));
//                else if (string.Equals(method, "POST", StringComparison.OrdinalIgnoreCase))
//                    response = httpClient.PostAsync(new Uri(url), new FormUrlEncodedContent(nameValueCollection));
//                else
//                    throw new ArgumentException("method");
//                return response.Result.Content.ReadAsStringAsync().Result;
//            }
//        }
//    }
//}

//#endif