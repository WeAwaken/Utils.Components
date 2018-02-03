using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Awaken.Utils.Widgets
{
    /// <summary>
    /// 操作系统
    /// </summary>
    public class OSSource
    {
        private static volatile Dictionary<Source, string> _sources = new Dictionary<Source, string>();

        private static readonly object _locker = new object();

        /// <summary>
        /// 客户端操作系统
        /// </summary>
        public enum Source
        {
            Windows = 0,
            
            Mac = 1,
            
            iPhone = 2,
            
            Android = 3,
            
            Other = 4
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
                            {Source.Windows,"Windows"},
                            {Source.Android,"Android"},
                            {Source.iPhone,"iPhone"},
                            {Source.Mac,"Mac"},
                            {Source.Other,"Other"}
                        };
                        }
                    }
                }

                return _sources;
            }
        }

    }
}
