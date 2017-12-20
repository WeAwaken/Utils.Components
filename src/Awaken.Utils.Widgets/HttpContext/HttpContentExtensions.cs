// Copyright (c) rigofunc and lotosbin. All rights reserved.

using System.Threading.Tasks;
using Newtonsoft.Json;

namespace System.Net.Http {
    /// <summary>
    /// Represents extensions for <see cref="HttpContent"/>.
    /// </summary>
    public static class HttpContentExtensions
    {
        /// <summary>
        /// Reads as asynchronous.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="content">The content.</param>
        /// <returns>A <see cref="Task{TResult}"/> represents the put operation.</returns>
        public async static Task<T> ReadAsAsync<T>(this HttpContent content) {

            var value = await content.ReadAsStringAsync();

            if (typeof(T) == typeof(string))
            {
                dynamic temp = value;
                return temp;
            }
            return JsonConvert.DeserializeObject<T>(value);

        }
    }
}