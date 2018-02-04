using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Awaken.Utils.Cache
{
    public interface IRedisCache
    {
        /// <summary>
        /// 创建Redis连接管理
        /// </summary>
        /// <returns></returns>
        ConnectionMultiplexer Connection { get; }

        /// <summary>
        /// 获取Redis IDatabase; Obtain an interactive connection to a database inside redis
        /// </summary>        
        /// <returns></returns>
        IDatabase GetDatabase(int db = -1, object asyncState = null);

        /// <summary>
        /// 获取Redis Server [0]
        /// </summary>
        /// <param name="index">Endpoints index</param>
        /// <returns></returns>
        IServer GetServer(int index);

        /// <summary>
        /// 存入 String
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        Task<bool> Set(string key, string value);

        /// <summary>
        /// 存入 Object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        Task<bool> Set<T>(string key, T value) where T : class, new();

        /// <summary>
        /// 存入 String Expiry
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="expiry">过期时间</param>
        Task<bool> Set(string key, string value, TimeSpan expiry);

        /// <summary>
        /// 存入 Object Expiry
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="expiry">过期时间</param>
        Task<bool> Set<T>(string key, T value, TimeSpan expiry) where T : class, new();

        /// <summary>
        /// 批量新增（NotWait，如需等待请自行构建Task.WaitAll()）
        /// </summary>
        /// <param name="keyValues"></param>
        /// <returns></returns>
        void BatchSet(List<RedisKeyValue> keyValues);

        /// <summary>
        ///  批量新增（NotWait，如需等待请自行构建Task.WaitAll()）
        /// </summary>
        /// <param name="keyValues">键</param>
        /// <param name="expiry">过期时间</param>
        void BatchSet(List<RedisKeyValue> keyValues, TimeSpan expiry);

        /// <summary>
        /// 获取对象
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<string> Get(string key);

        /// <summary>
        /// 获取对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<T> Get<T>(string key) where T : class, new();

		/// <summary>
		/// 批量获取 string 对象
		/// </summary>
		/// <param name="keys"></param>
		/// <returns></returns>
		Task<List<string>> BatchGet(List<string> keys);

		/// <summary>
		/// 批量获取 T 对象
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="keys"></param>
		/// <returns></returns>
		Task<List<T>> BatchGet<T>(List<string> keys) where T : class, new();

		/// <summary>
		/// 模糊查询 Fuzzy Search
		/// </summary>
		/// <param name="pattern"></param>
		/// <param name="pageSize">"7*" 则找到keys=7, 71 ,72 ,73</param>
		/// <returns></returns>
		Task<List<string>> Search(string pattern, int pageSize);

		/// <summary>
		/// 模糊查询
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="pattern">"7*" 则找到keys=7, 71 ,72 ,73</param>
		/// <param name="pageSize"></param>
		/// <returns></returns>
		Task<List<T>> Search<T>(string pattern, int pageSize) where T : class, new();
		        
        /// <summary>
        /// 获取对象
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<bool> Delete(string key);
    }
}
