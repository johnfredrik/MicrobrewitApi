using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microbrewit.Repository;

namespace Microbrewit.Api.Controllers
{
    [RoutePrefix("redis")]
    public class RedisController : ApiController
    {
        
        [Route("update")]
        public IHttpActionResult UpdateRedis()
        {
            IHopRepository hopRepository = new HopRepository();
            HopController hopController = new HopController(hopRepository);
            hopController.UpdateHops();
            return Ok();
        }

    }
}
