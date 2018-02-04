namespace Awaken.Utils.Cache
{
	public class RedisKeyValue
    {
        public string Key { set; get; }

        /// <summary>
        ///  JsonConvert.SerializeObject(T)
        /// </summary>
        public string JsonValue { set; get; }
    }
}
