using System;
using System.Collections.Generic;
using System.Text;

namespace Awaken.Utils.Widgets
{
    public class ApiRouteModel
    {
        /// <summary>
        /// Api Name Id
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Request Uri
        /// </summary>
        public string RequestUri { get; set; }
        /// <summary>
        /// Request Method
        /// </summary>
        public string Method { get; set; }
        /// <summary>
        /// Request NeedMethod
        /// </summary>
        public bool IsNeedAuth { get; set; }
    }
}
