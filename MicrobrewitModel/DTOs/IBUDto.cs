using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Microbrewit.Model.DTOs
{
    [ElasticType(Name = "ibu")]
    public class IBUDto
    {
        public int Id { get; set; }
        public int Standard { get; set; }
        public double Tinseth { get; set; }
        public double Rager { get; set; }
    }
}