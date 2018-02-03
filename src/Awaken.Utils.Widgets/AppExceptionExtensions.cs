using Awaken.Utils.Widgets;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class AppExceptionExtensions
    {
        /// <summary>
        /// 错误处理
        /// <para>
        /// 使用方法：app.UseExceptionHandler(errorOptions =>{do somethings});   
        /// </para>         
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseAppException(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(errorOptions =>
            {
                errorOptions.Run(async context =>
                {
                    context.Response.ContentType = "application/json; charset=utf-8"; // text/plain;

                    var error = context.Features.Get<IExceptionHandlerFeature>();

                    if (error != null)
                    {
                        var result = error.Error.InnerException ?? error.Error;

                        context.Response.StatusCode = (int)AppException.ErrorCode.ApplicationError;

                        var errorStatus = AppException.ErrorCode.ApplicationError.GetDescription();

                        if (error.Error is AppException)
                        {
                            // 自定义应用错误
                            var ex = error.Error as AppException;

                            // another error Status accordingly
                            context.Response.StatusCode = (int)ex.ErrorStatus;
                            errorStatus = ex.ErrorStatus.GetDescription();
                        }
                        else
                        {
                            // TODO: 系统错误 记录日志
                        }
                        // js:var result=eval('('+xhq.responseText+')')
                        await context.Response.WriteAsync($"{{\"error\":\"{errorStatus}\",\"message\":\"{result.Message}\"}}",Encoding.UTF8);

                    }

                });
            });

            return app;
        }
    }
}
