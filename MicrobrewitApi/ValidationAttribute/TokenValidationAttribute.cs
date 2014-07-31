using System;
using System.Linq;
using System.Net.Http;
using System.Web.Http.Filters;
using System.Web.Http.Controllers;
using Microbrewit.Api.Util;
using log4net;
using Newtonsoft.Json;
using Microbrewit.Model;
using System.IdentityModel.Tokens;
using StackExchange.Redis;
using Microbrewit.Model.DTOs;

namespace Microbrewit.Api.ValidationAttribute
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
                Encrypting.JWTValidation(token);               
                base.OnActionExecuting(actionContext);
            }
            catch (SecurityTokenValidationException ex)
            {
                //Log.Debug(ex);
                if (ex.Message.ToString().Contains("Jwt10305") || ex.Message.ToString().Contains("No token found in redis store"))
                {
                    actionContext.Response = new HttpResponseMessage(System.Net.HttpStatusCode.Forbidden)
                    {
                        Content = new StringContent("Authorization-Token Expired")
                    };
                   
                }
                else
                {

                    actionContext.Response = new HttpResponseMessage(System.Net.HttpStatusCode.Forbidden)
                    {
                        Content = new StringContent("Unauthorized User")
                    
                    };
                }
                return;
            }
        }
    }
}