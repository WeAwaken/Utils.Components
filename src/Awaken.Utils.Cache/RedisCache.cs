using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace Awaken.Utils.Cache
{
    /// <summary>
    /// Mysql Net 7.0.2 Later/5.7.2
    /// Access Data
    /// http://dev.mysql.com/doc/dev/connector-net/html/connector-net-x-devapi-getting-started.htm
    /// </summary>
    public class RedisCache : IRedisCache
    {
        private readonly RedisConnOptions _options;

        //static volatile
        private readonly Lazy<ConnectionMultiplexer> _lazyConnection;
               
        public RedisCache(IOptions<RedisConnOptions> optionsAccessor)
        {
            _options = optionsAccessor.Value;

            _lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
                 ConnectionMultiplexer.Connect(_options.ConnectionString)
            );
        }

        //private const string ConnName = "ConnectionStrings";        

        /// <summary>
        /// 获取 REDIS Connection
        /// <para>
        ///	await _redisCache.Database.StringSetAsync(key, value);
        ///	</para>
        /// </summary>
        /// <returns></returns>
        public ConnectionMultiplexer Connection
        {
            get
            {
                return _lazyConnection.Value;
            }
        }

        /// <summary>
        /// 获取Redis IDatabase
        /// </summary>        
        /// <returns></returns>
        public IDatabase Database
        {
            get {
                return Connection.GetDatabase();
            }
        }

        /// <summary>
        /// 获取Redis Server [0]
        /// </summary>
        /// <param name="index">Endpoints index</param>
        /// <returns></returns>        
        public IServer GetServer(int index=0)
		{
			return Connection.GetServer(Connection.GetEndPoints()[index]);
		}        

        /// <summary>
        /// 存入 String
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public async Task<bool> Set(string key, string value)
        {
            //using (var muxer = Create())
            //{
                //var db = Conn.GetDatabase();	
            return await Database.StringSetAsync(key, value);
            //}
        }

        /// <summary>
        /// 存入 Object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public async Task<bool> Set<T>(string key, T value) where T : class, new()
        {
            //var valueJson = await Task.Run(() => JsonConvert.SerializeObject(value));

            var valueJson = JsonConvert.SerializeObject(value);
            //using (var muxer = Create())
            //{
            //var db = Conn.GetDatabase();

            return await Database.StringSetAsync(key, valueJson);
            //}

        }

        /// <summary>
        /// 存入 String Expiry
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="expiry">过期时间</param>
        public async Task<bool> Set(string key, string value, TimeSpan expiry)
        {
            //using (var muxer = Create())
            //{
                //var db = Conn.GetDatabase();

            return await Database.StringSetAsync(key, value, expiry);
            //}
        }

        /// <summary>
        /// 存入 Object Expiry
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="expiry">过期时间</param>
        public async Task<bool> Set<T>(string key, T value, TimeSpan expiry) where T : class, new()
        {
            //var jsonValue = await Task.Run(() => JsonConvert.SerializeObject(value));

            var jsonValue = JsonConvert.SerializeObject(value);

            //using (var muxer = Create())
            //{
            //var db = muxer.GetDatabase();

            return await Database.StringSetAsync(key, jsonValue, expiry);
            //}
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <remarks>
        /// 如果需要等待自行构建 Task.WaitAll(tasks.ToArray());
        /// </remarks>
        /// <param name="keyValues"></param>
        /// <returns></returns>
        public void BatchSet(List<RedisKeyValue> keyValues)
        {
            //using (var muxer = Create())
            //{
            //    var db = muxer.GetDatabase();

            //List<Task> tasks = new List<Task>();

            var batch = Database.CreateBatch();

            foreach (RedisKeyValue keyValue in keyValues)
            {
                batch.StringSetAsync(keyValue.Key, keyValue.JsonValue).ConfigureAwait(false).GetAwaiter();

                //tasks.Add(batch.StringSetAsync(keyValue.Key, keyValue.JsonValue));
            }

            batch.Execute();

            //Task.WaitAll(tasks.ToArray());
            //}

            //return tasks;
        }

        /// <summary>
        ///  批量新增
        /// </summary>
        /// <remarks>
        /// 如果需要等待使用：Task.WaitAll(tasks.ToArray());
        /// </remarks>
        /// <param name="keyValues">键</param>       
        /// <param name="expiry">过期时间</param>
        public void BatchSet(List<RedisKeyValue> keyValues, TimeSpan expiry)
        {
            //using (var muxer = Create())
            //{
            //    var db = muxer.GetDatabase();

            //var tasks = new List<Task>();

            var batch = Database.CreateBatch();

            foreach (RedisKeyValue keyvalue in keyValues)
            {
                batch.StringSetAsync(keyvalue.Key, keyvalue.JsonValue, expiry).ConfigureAwait(false).GetAwaiter();
                //tasks.Add(batch.StringSetAsync(keyvalue.Key, keyvalue.JsonValue, expiry));

                //tasks.Add(task);
            }
            
            batch.Execute();

            //Task.WaitAll(tasks.ToArray());
            //}
            //return tasks;
        }

        /// <summary>
        /// 获取对象
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<string> Get(string key)
        {
            //using (var muxer = Create())
            //{
                //var db = muxer.GetDatabase();

            return await Database.StringGetAsync(key);
            
            //}
        }

        /// <summary>
        /// 获取对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns>T result can be null </returns>
        public async Task<T> Get<T>(string key) where T : class, new()
        {
			//string value;

            //using (var muxer = Create())
            //{
            //    var db = muxer.GetDatabase();

			var jsonValue = await Database.StringGetAsync(key);                
            //}

			if (!jsonValue.HasValue||jsonValue.IsNullOrEmpty) return null;

			return JsonConvert.DeserializeObject<T>(jsonValue.ToString());
		}

        /// <summary>
        /// 批量获取 string 对象
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public async Task<List<string>> BatchGet(List<string> keys)
        {
			//RedisKey[] redisKeys = keys.Select(p=> { //isnull.. RedisKey key = p;return key;}).ToArray();
			var redisKeys = keys.Select(p=>(RedisKey)p).ToArray();
            
            //using (var muxer = Create())
            //         {
            //             var db = muxer.GetDatabase();

            var redisValues = await Database.StringGetAsync(redisKeys);                
            //}

			return redisValues.Where(p=>p.HasValue).Select(p =>p.ToString()).ToList();			
		}

		/// <summary>
		/// 批量获取 T 对象
		/// </summary>
		/// <param name="keys"></param>
		/// <returns></returns>
		public async Task<List<T>> BatchGet<T>(List<string> keys) where T : class, new()
		{
			RedisKey[] redisKeys = keys.Select(p => (RedisKey)p).ToArray();
            		
            //using (var muxer = Create())
            //{
            //var db = muxer.GetDatabase();

            RedisValue[] values = await Database.StringGetAsync(redisKeys);
			//}

			//var tasks = values.Where(p => p.HasValue).Select(p =>
   //                 Task.Run(() => JsonConvert.DeserializeObject<T>(p))					 
   //             );

			//Task.WaitAll(tasks.ToArray());

			//return tasks.Select(p => p.Result).ToList();

            return values.Where(p => p.HasValue)
                    .Select(p =>JsonConvert.DeserializeObject<T>(p))
                    .ToList();
        }

        /// <summary>
        /// 模糊查询 Fuzzy Search
        /// </summary>
        /// <param name="pattern"></param>
        /// <param name="pageSize">"7*" 则找到keys=7, 71 ,72 ,73</param>
        /// <returns></returns>
        public async Task<List<string>> Search(string pattern, int pageSize) 
		{
			//using (var muxer = Create())
			//{
			var server = GetServer();

			//	var db = muxer.GetDatabase();

			var keys = server.Keys(Database.Database, pattern: pattern, pageSize: pageSize,flags: CommandFlags.None);

            RedisValue[] values = await Database.StringGetAsync(keys.ToArray(), CommandFlags.None);
			//}

			return values.Where(p=>p.HasValue).Select(p => p.ToString()).ToList();
		}

		/// <summary>
		/// 模糊查询 Fuzzy Search
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="pattern"></param>
		/// <param name="pageSize">"=7*" 则找到keys=7, 71 ,72 ,73</param>
		/// <returns></returns>
		public async Task<List<T>> Search<T>(string pattern, int pageSize) where T : class, new()
        {
            //using (var muxer = Create())
            //         {
            var server = GetServer();

            //var db = muxer.GetDatabase();

            var keys = server.Keys(Database.Database, pattern: pattern, pageSize: pageSize, flags: CommandFlags.None);

            RedisValue[] values = await Database.StringGetAsync(keys.ToArray(), CommandFlags.None);
            //}

            return values.Where(p => p.HasValue)
                    .Select(p => JsonConvert.DeserializeObject<T>(p))
                    .ToList();

   //         var tasks = values.Where(p => p.HasValue).Select(p =>Task.Run(() => JsonConvert.DeserializeObject<T>(p)));

			//Task.WaitAll(tasks.ToArray());

			//return tasks.Select(p => p.Result).ToList();
		}		
		       
        /// <summary>
        /// 删除对象
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<bool> Delete(string key)
        {
            //using (var muxer = Create())
            //{
            //    var db = muxer.GetDatabase();

                return await Database.KeyDeleteAsync(key);
            //}

        }

        #region connection manage

        private void MuxerConfigurationChanged(object sender, EndPointEventArgs e)
        {
            //Log("Configuration changed: " + e.EndPoint);
        }

        private void MuxerErrorMessage(object sender, RedisErrorEventArgs e)
        {
            //Log(e.EndPoint + ": " + e.Message);
        }

        private void MuxerConnectionRestored(object sender, ConnectionFailedEventArgs e)
        {
            //Log("Endpoint restored: " + e.EndPoint);
        }

        #endregion
    }
}
