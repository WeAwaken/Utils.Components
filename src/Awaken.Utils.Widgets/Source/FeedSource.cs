using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Awaken.Utils.Widgets
{
    public class FeedSource
    {
        private static volatile Dictionary<Source, string> _sources = new Dictionary<Source, string>();

        private static readonly object _locker = new object();

        /// <summary>
        /// 来源
        /// </summary>
        public enum Source
        {
            /// <summary>
            /// 【用户】桌面浏览器
            /// </summary>
            Web = 0,

            /// <summary>
            /// 【用户】手机浏览器
            /// </summary>
            Mobile = 2,

            /// <summary>
            /// 【用户】手机浏览器
            /// </summary>
            Weixin = 4,

            /// <summary>
            /// 【用户】安卓手机APP
            /// </summary>
            Android = 8,

            /// <summary>
            /// 【用户】苹果手机APP
            /// </summary>
            Ios = 16,

            /// <summary>
            /// 【管理员】企业通用客户端
            /// </summary>
            Admin = 32

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
                            {Source.Web,"桌面浏览器"},
                            {Source.Mobile,"手机浏览器"},
                            {Source.Weixin,"微信端"},
                            {Source.Ios,"苹果客户端"},
                            {Source.Android,"安卓客户端"},
                            {Source.Admin,"企业端"}
                        };
                        }
                    }
                }

                return _sources;
            }
        }

    }
}
