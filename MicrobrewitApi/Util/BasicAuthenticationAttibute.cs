using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using MicrobrewitModel;
using System.Security.Principal;
using log4net;


namespace MicrobrewitApi.Util
{
    public class BasicAuthenticationAttibute : ActionFilterAttribute
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            Log.Debug(actionContext.Request.Headers.Authorization);
            if(actionContext.Request.Headers.Authorization == null)
            {
                Log.Debug("Unauthorized");
                actionContext.Response = new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
            }
            else
            {
                Log.Debug("Authorizing");
                string authToken = actionContext.Request.Headers.Authorization.ToString();
                //string decodedToken = Encoding.UTF8.GetString(Convert.FromBase64String(authToken));

                Log.Debug("authToken: " + authToken);
                //Log.Debug("decodedToken" + decodedToken);

                //string username = authToken.Substring(0, authToken.IndexOf("."));
                //string password = authToken.Substring(authToken.IndexOf(".") + 1);                
                //UserCredentials userCredentials = null;
                //using (var context = new MicrobrewitApiContext())
                //{
                //    userCredentials = context.UserCredentials.Include("User").Where(u => u.Username.Equals(username)).FirstOrDefault();

                //}
                //if (userCredentials != null && password.Equals(Encrypting.Decrypt(userCredentials.Password,userCredentials.SharedSecret)))
                if(HttpContext.Current.User.Identity.IsAuthenticated)
                {
                   // Log.Debug(userCredentials.User.Email);
                   // HttpContext.Current.User = new GenericPrincipal(new ApiIdentity(userCredentials.User), new string[] { });

                    base.OnActionExecuting(actionContext);
                }
                else
                {
                    Log.Debug("Unauthorized");
                    actionContext.Response = new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
                }
            }
        }
    }
}