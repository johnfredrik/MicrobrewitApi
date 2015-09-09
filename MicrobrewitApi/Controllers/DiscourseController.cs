using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using log4net;
using Microbrewit.Service.Interface;

namespace Microbrewit.Api.Controllers
{
    [RoutePrefix("discourse")]
    public class DiscourseController : ApiController
    {
        private readonly IUserService _userService;
 

        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public DiscourseController(IUserService userService)
        {
            _userService = userService;
        }

        [Route("")]
        [HttpGet]
        public async Task<IHttpActionResult> ValidateDiscourse(string sso, string sig)
        {
            var identity = (ClaimsIdentity)User.Identity;
            //if(!identity.IsAuthenticated)
            //    return StatusCode(HttpStatusCode.Unauthorized);
            //string ssoSecret = "d836444a9e4084d5b224a60c208dce14"; //must match sso_secret in discourse settings
            string ssoSecret = "test";
            string checksum = getHash(sso, ssoSecret);
            if (checksum != sig)
                return BadRequest("Invalid");

            byte[] ssoBytes = Convert.FromBase64String(sso);
            string decodedSso = Encoding.UTF8.GetString(ssoBytes);

            NameValueCollection nvc = HttpUtility.ParseQueryString(decodedSso);

            string nonce = nvc["nonce"];

            //TODO: Add your own get user information
            //Ensure user is logged in by adding the [Authorize]   
            //Attribute to this controller method and validate the
            //user has permission to access the forum      

            //var user = await _userService.GetSingleAsync(identity.Name);

            //TODO: add null checks.
            string email = ConfigurationManager.AppSettings["email"];//identity.Claims.SingleOrDefault(s => s.Type == "email").Value;
            string username = ConfigurationManager.AppSettings["username"];//identity.Name;
            //string name = user.Name;
            string externalId = ConfigurationManager.AppSettings["username"]; ;//identity.Claims.SingleOrDefault(s => s.Type == "id").Value; ;

            string returnPayload = "nonce=" + HttpUtility.UrlEncode(nonce) +
                                   "&email=" + HttpUtility.UrlEncode(email) +
                                   "&external_id=" + HttpUtility.UrlEncode(externalId) +
                                   "&username=" + HttpUtility.UrlEncode(username);
                                   //"&name=" + HttpUtility.UrlEncode(name);

            string encodedPayload = Convert.ToBase64String(Encoding.UTF8.GetBytes(returnPayload));
            string returnSig = getHash(encodedPayload, ssoSecret);

            string redirectUrl = "http://discourse.asphaug.io" + "/session/sso_login?sso=" + encodedPayload + "&sig=" + returnSig;

            return Redirect(redirectUrl);
            //Log.Debug("Ok");
            //return Ok();
        }

        public string getHash(string payload, string ssoSecret)
        {
            var encoding = new System.Text.UTF8Encoding();
            byte[] keyBytes = encoding.GetBytes(ssoSecret);

            System.Security.Cryptography.HMACSHA256 hasher = new System.Security.Cryptography.HMACSHA256(keyBytes);

            byte[] bytes = encoding.GetBytes(payload);
            byte[] hash = hasher.ComputeHash(bytes);

            string ret = string.Empty;
            foreach (byte x in hash)
                ret += String.Format("{0:x2}", x);
            return ret;
        }   
    }
}
