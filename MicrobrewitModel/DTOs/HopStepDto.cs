using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Microbrewit.Model.DTOs
{
    [ElasticType(Name = "hopStep")]
    public class HopStepDto
    {
        public int HopId { get; set; }
        public int StepId { get; set; }
        public string Name { get; set; }
        public double Amount { get; set; }
        public double AAValue { get; set; }
        public DTO Origin { get; set; }
        public DTO HopForm { get; set; }
        public string FlavourDescription { get; set; }
        public IList<DTO> Flavours { get; set; }


    }
}