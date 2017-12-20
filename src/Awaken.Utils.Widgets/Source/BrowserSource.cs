using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Awaken.Utils.Widgets
{
    public class BrowserSource
    {
        private static volatile Dictionary<Source, string> _sources = new Dictionary<Source, string>();

        private static readonly object _locker = new object();

        /// <summary>
        /// 用户来源
        /// </summary>
        public enum Source
        {
            /// <summary>
            /// 火狐浏览器
            /// </summary>
            Firefox = 0,

            /// <summary>
            /// IE浏览器
            /// </summary>
            IE = 1,

            /// <summary>
            /// 谷歌浏览器
            /// </summary>
            Chrome = 2,

            /// <summary>
            /// Safari
            /// </summary>
            Safari = 3,

            /// <summary>
            /// Opera
            /// </summary>
            Opera = 4,

            /// <summary>
            /// 微信浏览器
            /// </summary>
            Weixin = 5,

            /// <summary>
            /// 未知
            /// </summary>
            Unknow = 6

        }

        public static string SourceName(Source source)
        {
            return Sources[source];
        }

        public static Dictionary<Source, string> Sources
        {
            get
            {
                if (_sources.Count <= 0)
                {
                    lock (_locker)
                    {
                        if (_sources.Count <= 0)
                        {
                            _sources = new Dictionary<Source, string>
                        {
                            {Source.Chrome,"Chrome"},
                            {Source.Firefox,"Firefox"},
                            {Source.IE,"IE"},
                            {Source.Opera,"Opera"},
                            {Source.Safari,"Safari"},
                            {Source.Unknow,"Unknow"},
                            {Source.Weixin,"Weixin"}
                        };
                        }
                    }
                }

                return _sources;
            }
        }

        
    }
}
