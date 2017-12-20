using System;
using System.Collections.Generic;
using System.Text;

namespace Awaken.Utils.Widgets
{
    public class ErrorResult
    {
	    public ErrorResult()
        {
            Error = AppException.ErrorCode.BadRequest.GetDescription();
        }

	    public ErrorResult(string message)
        {
            Error = AppException.ErrorCode.BadRequest.GetDescription();
            Message = message;
        }

        public ErrorResult(string error, string message)
        {
            Error = error;
            Message = message;
        }

        public ErrorResult(AppException.ErrorCode error,string message)
        {
            Error = error.GetDescription();
            Message = message;
        }

        public string Error { get; set; }

        public string Message { get; set; }

        public override string ToString()
        {
            return string.Format("{{error:'{0}',message:'{1}'}}",
                Error,
                Message);
        }
    }
}
