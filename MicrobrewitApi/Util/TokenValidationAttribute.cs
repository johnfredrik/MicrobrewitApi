using System;
using System.Linq;
using System.Net.Http;
using System.Web.Http.Filters;
using System.Web.Http.Controllers;
using MicrobrewitApi.Util;
using log4net;

namespace MicrobrewitApi.Controllers
{
    public class TokenValidationAttribute : ActionFilterAttribute
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            string token;

            try
            {
                token = actionContext.Request.Headers.GetValues("Authorization-Token").First();
                Log.Debug("token: " + token);
            }
            catch (Exception)
            {
                actionContext.Response = new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest)
                {
                    Content = new StringContent("Missing Authorization-Token")
                };
                return;
            }

            try
            {                
                AuthorizedUserRepository.GetUsers().First(x => x.Token.Equals(Encrypting.Decrypt(token,x.SharedSecret)));
                base.OnActionExecuting(actionContext);
            }
            catch (Exception ex)
            {
                Log.Debug(ex);
                actionContext.Response = new HttpResponseMessage(System.Net.HttpStatusCode.Forbidden)
                {
                    Content = new StringContent("Unauthorized User")
                    
                };
                return;
            }
        }
    }
}