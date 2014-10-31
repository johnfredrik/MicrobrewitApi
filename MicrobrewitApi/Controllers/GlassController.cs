using AutoMapper;
using Microbrewit.Model;
using Microbrewit.Model.DTOs;
using Microbrewit.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Microbrewit.Api.Controllers
{
    [RoutePrefix("glasses")]
    public class GlassController : ApiController
    {
        private IGlassRepository _glassRepository;
        private Elasticsearch.ElasticSearch _elasticsearch;

        public GlassController(IGlassRepository glassRepository)
        {
            this._glassRepository = glassRepository;
            this._elasticsearch = new Elasticsearch.ElasticSearch();
        }

        [Route("")]
        public async Task<GlassCompleteDto> GetGlasses()
        {
            var glasses = await _glassRepository.GetAllAsync();
            var glassesDto = Mapper.Map<IList<Glass>,IList<GlassDto>>(glasses);
            var result = new GlassCompleteDto();
            result.Glasses = glassesDto;
            return result;
        }
    }
}
