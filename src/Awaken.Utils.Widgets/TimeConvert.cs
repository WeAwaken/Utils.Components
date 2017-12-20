using System;
using System.Collections.Generic;
using System.Text;

namespace Awaken.Utils.Widgets
{
    /// <summary>
    /// 时间转换组件
    /// </summary>
    public static class TimeConvert
    {   
        /// <summary>
        /// Unix Time Convert To DateTime
        /// </summary>
        /// <param name="unixTime"></param>
        /// <returns></returns>
        public static DateTime UnixTimeToTime(long unixTime)
        {
            return DateTimeOffset.FromUnixTimeSeconds(unixTime)
                    .DateTime.ToLocalTime();            
        }

        /// <summary>
        /// DateTime Convert To Unix Time
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static long TimeToUnixTime(DateTime dateTime)
        {
            return new DateTimeOffset(dateTime)
                        .ToUnixTimeSeconds();    
        }
    }
}
