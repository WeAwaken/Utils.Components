using IdentityModel.Client;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Awaken.Utils.Widgets
{
    public class ApiEndPoints
    {
        private readonly ApiRouteOptions _options;

        private readonly Lazy<List<ApiRouteModel>> _lazyEndpoints;

        /// <summary>
        /// 访问键值
        /// </summary>
        private static string _accessToken;
        /// <summary>
        /// 过期时间
        /// </summary>
        private static DateTime _expireTime;        

        /// <summary>
        /// 默认API列表[有效]
        /// </summary>
        private const string ApiRouteEndPoint = "/setting/apiroute";

        /// <summary>
        /// Api EndPoints
        /// </summary>
        /// <param name="optionsAccessor"></param>
        public ApiEndPoints(IOptions<ApiRouteOptions> optionsAccessor)
        {
            _options = optionsAccessor.Value;

            // 延迟装载Api列表
            _lazyEndpoints = new Lazy<List<ApiRouteModel>>(() =>
                  LoadEndpointsAsync().GetAwaiter().GetResult()
            ); 
        }

        /// <summary>
        /// Get BaseAddress
        /// </summary>
        public string BaseAddress { get { return _options.BaseAddress; } }

        /// <summary>
        /// Api End Points
        /// </summary>
        public List<ApiRouteModel> EndPoints
        {
            get {
                return _lazyEndpoints.Value;
            }
        }

        /// <summary>
        /// Get Api Service EndPoint
        /// <para>Url with BaseAddress</para>
        /// </summary>
        /// <param name="apiName"></param>
        /// <returns>Url with BaseAddress</returns>
        public string this[string apiName]
        {
            get {
                
                var route = EndPoints.FirstOrDefault(p => p.Id == apiName);

                return _options.BaseAddress + route.RequestUri;
            }
        }

        public ApiRouteModel Get(string apiName)
        {
            return EndPoints.FirstOrDefault(p => p.Id == apiName);
        }

        /// <summary>
        /// init EndPoints
        /// </summary>
        /// <returns></returns>
        private async Task<List<ApiRouteModel>> LoadEndpointsAsync()
        {           
            var apiUrl = _options.BaseAddress + ApiRouteEndPoint;

            var client = HttpClientBuilder.CreateHttpClient(false);

            // TODO:Product 环境下必须打开验证
            // client.SetBearerToken(await GetAccessTokenAsync());
            for (var i = 0; i < 10; i++)
            {
                var response = await client.GetAsync(apiUrl);

                if (!response.IsSuccessStatusCode)
                {
                    //失败后继续重试 10次
                    continue;
                }
                List<ApiRouteModel> result;

                try {
                    result = await response.Content.ReadAsAsync<List<ApiRouteModel>>();
                }
                catch(Exception ex){
                    // log 日志 报警
                    continue;
                }
                return result;
            }

            throw new AppException("Register Api Uri List Get Faild:Connect Timeout");

        }

        /// <summary>
        /// 获取AccessToken
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetAccessTokenAsync()
        {
            if (!string.IsNullOrEmpty(_accessToken) && _expireTime > DateTime.Now)
            {
                return _accessToken;
            }
            var discoClient = new DiscoveryClient(_options.Authority);//

            // NOTICE:仅限开发环境不使用SSL
            if (!_options.Authority.Contains("https")) {
                discoClient.Policy.RequireHttps = false;
            }            
            
            var discovery = await discoClient.GetAsync();            
            
            var tokenClient = new TokenClient(discovery.TokenEndpoint, _options.ClientId, _options.ClientSecret);
            var tokenResponse = await tokenClient.RequestClientCredentialsAsync(_options.ClientScope);
            
            if (tokenResponse.IsError)
            {
                //Console.WriteLine(tokenResponse.Error);
                throw new AppException("Register Api Uri List Get Faild:Auth Timeout");                
            }

            //将过期时间提前60秒 防止过期访问
            _expireTime = DateTime.Now.AddSeconds( tokenResponse.ExpiresIn-60);

            _accessToken = tokenResponse.AccessToken;

            return _accessToken;
            
        }

    }
}
