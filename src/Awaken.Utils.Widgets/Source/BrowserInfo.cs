using System.Linq;
using System.Text.RegularExpressions;

namespace Awaken.Utils.Widgets
{
    /// <summary>
    /// 浏览器信息
    /// </summary>
    public static class BrowserInfo
    {
        public static volatile string[] mobileAgents = { "iphone", "android", "phone", "mobile", "wap", "opera mobi", "opera mini", "ucweb", "windows ce", "symbian", "blackberry", "nokia", "samsung", "meizu", "motorola", "wap-", "wapa", "wapi", "wapp", "wapr", };

        /// <summary>
        /// 判断是否是手机访问
        /// </summary>
        /// <returns></returns>
        public static bool IsMobile(string userAgent)
        {
            if (!string.IsNullOrEmpty(userAgent))
            {
                userAgent = userAgent.ToLower();

                if (mobileAgents.Any(t => userAgent.IndexOf(t) >= 0))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 判断手机浏览器访问者来源
        /// </summary>
        /// <returns></returns>
        public static FeedSource.Source SourcePhoneWeb(string userAgent)
        {
            // 默认端
            if (string.IsNullOrEmpty(userAgent)) return FeedSource.Source.Web;

            userAgent = userAgent.ToLower();

            // 微信
            if (userAgent.Contains("micromessenger"))
                return FeedSource.Source.Weixin;

            //苹果
            if (userAgent.Contains("iphone"))
                return FeedSource.Source.Ios;

            //安卓
            if (userAgent.Contains("android"))
                return FeedSource.Source.Android;

            return FeedSource.Source.Web;
        }

        /// <summary>
        /// 判断浏览器访问者来源
        /// </summary>
        /// <returns></returns>
        public static string SourceWeb(string userAgent)
        {
            Regex firefoxReg = new Regex(@"firefox\/(\d)*");
            Regex ieReg = new Regex(@"ms(ie)\/(\d)*");
            Regex chromeReg = new Regex(@"chrome\/(\d)*");
            Regex safariReg = new Regex(@"version\/(\d+).* mobile\/(\w+)* safari\/(\d+).+");
            Regex operaReg = new Regex(@"opera\/(\d)*");
            Regex wxReg = new Regex(@"micromessenger\/(\d)*");

            if (wxReg.IsMatch(userAgent))
            {
                return BrowserSource.SourceName(BrowserSource.Source.Weixin);
            }
            if (firefoxReg.IsMatch(userAgent))
            {
                return BrowserSource.SourceName(BrowserSource.Source.Firefox);
            }
            else if (ieReg.IsMatch(userAgent))
            {
                return BrowserSource.SourceName(BrowserSource.Source.IE);
            }
            else if (chromeReg.IsMatch(userAgent))
            {
                return BrowserSource.SourceName(BrowserSource.Source.Chrome);
            }
            else if (safariReg.IsMatch(userAgent))
            {
                return BrowserSource.SourceName(BrowserSource.Source.Safari);
            }
            else if (operaReg.IsMatch(userAgent))
            {
                return BrowserSource.SourceName(BrowserSource.Source.Opera);
            }
            else
            {
                return BrowserSource.SourceName(BrowserSource.Source.Unknow);
            }
        }

        /// <summary>
        /// 操作系统
        /// </summary>
        /// <param name="userAgent"></param>
        /// <returns></returns>
        public static string SourceOS(string userAgent)
        {
            Regex windowsReg = new Regex("windows[^abc]*;+");
            Regex macReg = new Regex("mac;+");
            Regex iphoneReg = new Regex("iphone;+");
            Regex andriodReg = new Regex("andriod;+");

            if (windowsReg.IsMatch(userAgent))
            {
                return OSSource.SourceName(OSSource.Source.Windows);
            }
            else if (macReg.IsMatch(userAgent))
            {
                return OSSource.SourceName(OSSource.Source.Mac);
            }
            else if (iphoneReg.IsMatch(userAgent))
            {
                return OSSource.SourceName(OSSource.Source.iPhone);
            }
            else if (andriodReg.IsMatch(userAgent))
            {
                return OSSource.SourceName(OSSource.Source.Android);
            }
            else
            {
                return OSSource.SourceName(OSSource.Source.Other);
            }
        }


    }
}

