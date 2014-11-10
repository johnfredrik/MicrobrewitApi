using Microbrewit.Model;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Microbrewit.Model.DTOs
{
    [ElasticType(Name = "boilStep")]
    public class BoilStepDto
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public int Length { get; set; }
        public int Volume { get; set; }
        public IList<HopStepDto> Hops { get; set; }
        public IList<FermentableStepDto> Fermentables { get; set; }
        public IList<OtherStepDto> Others { get; set; }
        public string Notes { get; set; }

    }
}