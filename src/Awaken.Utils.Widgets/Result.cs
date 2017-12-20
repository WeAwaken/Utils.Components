namespace Awaken.Utils.Widgets
{
	/// <summary>
	/// 返回结果 
    /// 已经不推荐使用 请尽量使用标准返回值
	/// </summary>
	public class Result<T> where T:class,new()
	{
		public Result() { }

        /// <summary>
        /// 执行返回
        /// </summary>
        /// <param name="data"></param>
        /// <param name="message"></param>
        public Result(T data,string message="")
        {
            Message = message;
            Data = data;
        }

        ///// <summary>
        ///// 一般情况返回
        ///// </summary>
        ///// <param name="code"></param>
        ///// <param name="message"></param>
        ///// <param name="data"></param>
        public Result(string message, T data = null)
        {
            Message = message;
            Data = data;
        }        

		/// <summary>
		/// 信息
		/// </summary>
		public string Message { get; set; }

		/// <summary>
		/// 数据 存储返回的相关对象
		/// [{"name": "a","accountid": "1","role": "user","tenant": "t"},
		/// {"name": "b","accountid": "2","role": "user","tenant": "tt"}]
		/// </summary>
		public T Data { get; set; }
	}
    
}
