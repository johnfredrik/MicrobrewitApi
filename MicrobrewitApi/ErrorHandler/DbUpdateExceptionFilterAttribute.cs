using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Filters;

namespace Microbrewit.Api.ErrorHandler
{
    public class DbUpdateExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext.Exception is DbUpdateException)
            {
                
                var error = "Error occurred:" + actionExecutedContext.Exception.ToString();
                
                var httpResponse = new HttpResponseMessage(HttpStatusCode.BadRequest);
                httpResponse.Content = new StringContent(actionExecutedContext.Exception.Message);
                actionExecutedContext.Response = httpResponse;
            }
        }
    }
}