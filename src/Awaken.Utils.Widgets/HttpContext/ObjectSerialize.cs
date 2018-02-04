using Newtonsoft.Json;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

namespace Awaken.Utils.Widgets
{
	/// <summary>
	/// 对象 和 byte[]转换工具
	/// </summary>
	public static class ObjectSerialize
	{
		/// <summary>
		/// 将Object转换成Json string [异步方法]
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="value"></param>
		/// <returns></returns>
		public static async Task<string> SerializeAsync<T>(T value)
		{
			return await Task.Run(() => JsonConvert.SerializeObject(value));
		}

		/// <summary>
		/// 将JSON字符串转换成Object [异步方法]
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="valueJson"></param>
		/// <returns></returns>
		public static async Task<T> DeserializeAsync<T>(string valueJson)
		{
			return await Task.Run(() => JsonConvert.DeserializeObject<T>(valueJson));
		}

		/// <summary> 
		/// 将一个对象序列化，返回一个byte[]         
		/// </summary> 
		/// <param name="value">序列化的对象</param>         
		/// <returns></returns> 
		public static async Task<byte[]> ToBytesAsync<T>(T value)
		{
			//in .net core 2.0
			//System.Runtime.Serialization.Formatters.Binary
			ArraySegment<byte> bytes;

			using (MemoryStream ms = new MemoryStream())
			{
				var formatter = new BinaryFormatter();

				formatter.Serialize(ms, value);

				ms.TryGetBuffer(out bytes);
			}

			return bytes.Array;

            /* 效率低
			var json = await SerializeStringAsync(value);

			byte[] val = System.Text.Encoding.UTF8.GetBytes(json);

			return val;
            */

		}

		/// <summary> 
		/// 将一个序列化后的byte[]数组还原         
		/// </summary>
		/// <param name="bytes"></param>         
		/// <returns></returns> 
		public static async Task<T> ToObjectAsync<T>(byte[] bytes)
		{
            using (var ms = new MemoryStream(bytes))
            {
                var formatter = new BinaryFormatter();

                return (T)formatter.Deserialize(ms);                
            }

            /* 效率低
			var json = System.Text.Encoding.UTF8.GetString(bytes);

			return await DeserializeObjectAsync<T>(json);*/

        }
    }
}
