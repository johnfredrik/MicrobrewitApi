using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using log4net;
using System.Text;
using Microbrewit.Model;
using System.Net.Http;
using System.IdentityModel.Tokens;
using ServiceStack.Redis.Generic;
using ServiceStack.Text;
using Microbrewit.Model.DTOs;
using ServiceStack.Redis;

namespace Microbrewit.Api.Util
{
    public class TokenInvalidationAttribute : ActionFilterAttribute
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            string token;
            try
            {
                token = actionContext.Request.Headers.GetValues("Authorization-Token").First();
            }
            catch (Exception)
            {
                actionContext.Response = new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest)
                {
                    Content = new StringContent("Missing Authorization-Token")
                };
                return;
            }
            Encrypting.JWTInvalidateToken(token);
            base.OnActionExecuting(actionContext);
        }
    }
}