using Library.API.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.API.Helpers
{
    /// <summary>
    /// 
    /// </summary>
    public class CustomUnauthorizedResult : JsonResult
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public CustomUnauthorizedResult(string message) : base(new AuthorizeErrorViewModel(message))
        {
            StatusCode = StatusCodes.Status401Unauthorized;
        }
    }
}
