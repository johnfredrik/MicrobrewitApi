using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nest;

namespace Microbrewit.Model.DTOs
{
    [ElasticType(Name = "abv")]
    public class ABVDto
    {
        public int Id { get; set; }
        public double Standard { get; set; }
        public double Miller { get; set; }
        public double Advanced { get; set; }
        public double AdvancedAlternative { get; set; }
        public double Simple { get; set; }
        public double AlternativeSimple { get; set; }
    }
}