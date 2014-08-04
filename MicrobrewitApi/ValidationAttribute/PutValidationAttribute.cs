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
using StackExchange.Redis;
using Microbrewit.Model.DTOs;

using System.Configuration;
using Microbrewit.Api.Util;


namespace Microbrewit.Api.ValidationAttribute
{
    public class PutValidationAttribute : ActionFilterAttribute
    {
        #region Private Fields
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly int expire = int.Parse(ConfigurationManager.AppSettings["expire"]);
        private static readonly string redisStore = ConfigurationManager.AppSettings["redis"];


        #endregion

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var authentication = actionContext.Request.Headers.Authorization.Parameter.ToString();
            Log.Debug("auth: " + authentication);
            string decodedToken = Encoding.UTF8.GetString(Convert.FromBase64String(authentication));
            Log.Debug("decodedToken: " + decodedToken);
            string username = decodedToken.Substring(0, decodedToken.IndexOf(":"));
            string password = decodedToken.Substring(decodedToken.IndexOf(":") + 1);

            UserCredentials userCredentials = null;
            using (var context = new MicrobrewitContext())
            {
                userCredentials = context.UserCredentials.Include("User").Where(u => u.Username.Equals(username)).FirstOrDefault();

            }
            if (userCredentials != null && Encrypting.ValidatePassword(userCredentials, password))
            {
                base.OnActionExecuting(actionContext);


                //JwtHeader jwtHeader = new JwtHeader{{typ, JWT})
                var jwtString = Encrypting.JWTDecrypt(userCredentials.User);
                using (var redis = ConnectionMultiplexer.Connect(redisStore))
                {

                    var redisClient = redis.GetDatabase();

                    var timespan = TimeSpan.FromMinutes(expire);
                    redisClient.StringSet(jwtString, userCredentials.Username);
                    redisClient.KeyExpire(jwtString, timespan, flags: CommandFlags.FireAndForget);
                    // seting token to header
                    HttpContext.Current.Response.AddHeader("Authorization-Token", jwtString);
                }
            }
            else
            {
                // returns unauthorised if not password maches.
                actionContext.Response = new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
            }
        }
    }
}