using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;

namespace Awaken.Utils.Widgets
{
    public class HttpClientPool
    {
        private static volatile HttpClientPool _instance;

        private static readonly object _locker = new object();

        /// <summary>
        /// volatile 多线程中保持同步一个结果
        /// </summary>
        private static volatile HttpClient _httpClient;

        private HttpClientPool()
        {
            _httpClient = CreateClient();
        }

        public static HttpClientPool Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_locker)
                    {
                        if (_instance == null)
                        {
                            _instance = new HttpClientPool();
                        }
                    }
                }

                return _instance;
            }
        }

        /// <summary>
        /// 获取httpClient
        /// </summary>
        /// <returns></returns>
        public HttpClient GetClient()
        {
            if (_httpClient == null)
            {             
                _httpClient = CreateClient();
            }

            return _httpClient;
        }

        private HttpClient CreateClient()
        {
            var handler = new HttpClientHandler()
            {
                AutomaticDecompression = DecompressionMethods.GZip                
            };

            return new HttpClient(handler,false);
        }

    }
}
