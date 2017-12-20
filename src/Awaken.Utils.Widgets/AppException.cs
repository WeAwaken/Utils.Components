using System;
using System.ComponentModel;

namespace Awaken.Utils.Widgets
{
    /// <summary>
    /// return {"error":"description for developer","message":" message for user"}
    /// </summary>
    public class AppException :Exception
    {
        public ErrorCode ErrorStatus{ get;set; }

        /// <summary>
        /// 应用程序异常
        /// source code = httpStatusCode
        /// </summary>
        /// <param name="message"></param>
        /// <param name="errorCode"></param>
        public AppException(string message, ErrorCode errorCode = ErrorCode.BadRequest)
            : base(message)
        {
            ErrorStatus = errorCode;
        }

        /// <summary>
        /// 应用程序异常:带内部错误
        /// </summary>
        /// <param name="message"></param>
        /// <param name="errorCode"></param>
        /// <param name="innerException"></param>
        public AppException(string message, ErrorCode errorCode, Exception innerException)
            : base(message, innerException)
        {
            ErrorStatus = errorCode;
        }

        /// <summary>
        /// 错误资源代码 ~= Http Status Code
        /// </summary>
        public enum ErrorCode
        {
            /// <summary>
            /// 400 参数缺失或格式错误
            /// </summary>
            [Description("Invalid Parameters")]
            BadRequest = 400,

            /// <summary>
            /// 401 未授权的请求
            /// </summary>
            [Description("Unauthorized")]
            Unauthorized = 401,

            /// <summary>
            /// 402 请求失败,业务错误
            /// </summary>
            [Description("Request Failed")]
            RequestFailed = 402,

            /// <summary>
            /// 403 请求被拒绝，权限不够
            /// </summary>
            [Description("No Role Forbidden")]
            Forbidden = 403,

            /// <summary>
            /// 408 请求超时
            /// </summary>
            [Description("Request Timeout")]
            RequestTimeout = 408,

            #region 自定义错误 HTTP STATUS CODE >=480

            /// <summary>
            /// 480 版本过期
            /// </summary>
            [Description("Expired Version")]
            ExpiredVersion = 480,

            /// <summary>
            /// 481 有未完成的订单 是否继续
            /// </summary>
            [Description("Exist Unpay Order")]
            UnpayOrder = 481,

            /// <summary>
            /// 482 预设计的错误 业务处理使用
            /// </summary>
            [Description("Predictive Error")]
            Predict = 482

            #endregion
        }       


    }

    
}