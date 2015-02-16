using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Microbrewit.Api.Controllers
{
    [RoutePrefix("discourse")]
    public class DiscourseController : ApiController
    {
        [HttpGet]
        public void ValidateDiscourse(string sso, string sig)
        {
            
        } 
    }
}
