using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Awaken.Utils.Widgets
{
    /// <summary>
    /// HttpClient Builder
    /// https://aspnetmonsters.com/2016/08/2016-08-27-httpclientwrong/
    /// </summary>
    public static class HttpClientBuilder
    {
        /// <summary>
        /// Create HttpClient[WithNoBaseAddress]
        /// </summary>
        /// <param name="keepAlive"></param>
        /// <returns></returns>
        public static HttpClient CreateHttpClient(bool keepAlive = true)
        {
            var client = HttpClientPool.Instance.GetClient();

            // 去除BaseAddress
            if (client.BaseAddress != null)
            {
                try
                {
                    client.BaseAddress = null;
                }
                catch{}                
            }

            client.DefaultRequestHeaders.Accept.Clear();

            //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // 去除长连接
            client.DefaultRequestHeaders.Connection.Clear();// Remove("keep-alive");

            if (keepAlive)
            {
                client.DefaultRequestHeaders.Connection.Add("keep-alive");
            }
            // 清理授权
            if (client.DefaultRequestHeaders.Authorization != null)
            {
                client.DefaultRequestHeaders.Authorization = null;
            }            

            return client;
        }

        /// <summary>
        /// Create HttpClient[NotWithAuth]
        /// </summary>
        /// <param name="baseUri">HttpClient BaseAddress URI</param>
        /// <param name="keepAlive"></param>
        /// <returns></returns>
        public static HttpClient CreateHttpClient(string baseUri, bool keepAlive = true)
        {
            var client = CreateHttpClient(keepAlive);

            if (!string.IsNullOrEmpty(baseUri)) client.BaseAddress = new Uri(baseUri);

            return client;
        }

        /// <summary>
        /// Create HttpClient[WithAuth]
        /// </summary>
        /// <param name="baseUri">tip:can be null :) </param>
        /// <param name="bearerToken"></param>
        /// <param name="keepAlive"></param>
        /// <returns></returns>
        public static HttpClient CreateHttpClient(string baseUri, string bearerToken, bool keepAlive = true)
        {
            var client = CreateHttpClient(baseUri, keepAlive);

            //client.SetBearerToken(bearerToken);
            if (!string.IsNullOrEmpty(bearerToken))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);
            }

            return client;

        }

        /// <summary>
        /// Create HttpClient[WithAuth]
        /// </summary>
        /// <param name="baseUri"></param>
        /// <param name="scheme">{Basic , Bearer}</param>
        /// <param name="token">{Access Token}</param>
        /// <param name="keepAlive"></param>
        /// <returns></returns>
        public static HttpClient CreateHttpClient(string baseUri, string scheme, string token, bool keepAlive = true)
        {
            var client = CreateHttpClient(baseUri, keepAlive);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme, token);

            return client;
        }

        /// <summary>
        /// HttpClient init [提高访问速度]
        /// </summary>
        /// <param name="httpClient"></param>
        /// <returns></returns>
        public static async Task PreInit(HttpClient httpClient)
        {
            if (httpClient.BaseAddress == null)
            {
                return;
            }
            try
            {
                // 预热提高访问速度
                var preinit = await httpClient.SendAsync(new HttpRequestMessage
                {
                    Method = new HttpMethod("HEAD"),

                    RequestUri = new Uri("")

                });
                preinit.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                var error = ex;
                // TODO:记录日志 报告错误
            }
        }
    }
}
