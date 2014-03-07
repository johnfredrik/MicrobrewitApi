using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using log4net;
using System.Text;
using MicrobrewitModel;
using System.Net.Http;
using System.IdentityModel.Tokens;

namespace MicrobrewitApi.Util
{
    public class LoginValidationAttribute : ActionFilterAttribute
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var authentication = actionContext.Request.Headers.Authorization.Parameter.ToString();
            Log.Debug("auth: " + authentication);
            string decodedToken = Encoding.UTF8.GetString(Convert.FromBase64String(authentication));
            Log.Debug("decodedToken: " + decodedToken);
            string username = decodedToken.Substring(0, decodedToken.IndexOf(":"));
            string password = decodedToken.Substring(decodedToken.IndexOf(":") + 1);

            UserCredentials userCredentials = null;
            using (var context = new MicrobrewitApiContext())
            {
                userCredentials = context.UserCredentials.Include("User").Where(u => u.Username.Equals(username)).FirstOrDefault();

            }
            if (userCredentials != null && password.Equals(Encrypting.Decrypt(userCredentials.Password, userCredentials.SharedSecret)))
            {
                base.OnActionExecuting(actionContext);

               
                //JwtHeader jwtHeader = new JwtHeader{{typ, JWT})
                var jwtString = Encrypting.JWTDecrypt(userCredentials.User);
                HttpContext.Current.Response.AddHeader("Authorization-Token", jwtString);
            }
            else
            {
                Log.Debug("Unauthorized");
                actionContext.Response = new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
            }
        }
    }
}