using Awaken.Utils.Cache;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class RedisCacheExtensions
    {
		/// <summary>
		/// Redis Cache Extensions
		/// </summary>
		/// <param name="serviceCollection"></param>
		/// <param name="optionsAction"></param>
		/// <returns></returns>
        public static IServiceCollection AddRedisCache(
            this IServiceCollection serviceCollection,
            Action<RedisConnOptions> optionsAction)
        {
            //ConnectionOptions options=new {};
            serviceCollection.Configure(optionsAction);

            // 缓存仓库构建单例
            serviceCollection.AddSingleton<IRedisCache, RedisCache>();            

            return serviceCollection;

        }
        
    }
}
