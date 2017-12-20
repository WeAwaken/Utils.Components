namespace Awaken.Utils.Widgets
{
    /// <summary>
    /// Api Register's Options
    /// </summary>
    public class ApiRouteOptions
    {
        /// <summary>
        /// Api BaseAddress Uri
        /// </summary>
        public string BaseAddress { get; set; }        

        /// <summary>
        /// 身份认证地址
        /// </summary>
        public string Authority { get; set; }

        /// <summary>
        /// 消费客户端ID
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// 消费客户端账号
        /// </summary>
        public string ClientSecret { get; set; }

        /// <summary>
        /// 消费客户端Scope
        /// </summary>
        public string ClientScope { get; set; }

        /// <summary>
        /// 实例化对象本身
        /// .NET CORE BUG 
        /// Action 不能传递引用对象
        /// </summary>
        /// <param name="options"></param>
        public void Instant(ApiRouteOptions options)
        {
            BaseAddress = options.BaseAddress;
            Authority = options.Authority;
            ClientId = options.ClientId;
            ClientSecret = options.ClientSecret;
            ClientScope = options.ClientScope;           
        }
    }

    /*
    public class HttpContextOptions
    {        
        private static IHttpContextAccessor _httpContextAccessor;

        public static void Configure(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public static HttpContext HttpContext
        {
            get
            {               

                return _httpContextAccessor.HttpContext;
            }
        }
        
    }*/
}
