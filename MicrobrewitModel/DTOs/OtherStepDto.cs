using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Microbrewit.Model.DTOs
{
    [ElasticType(Name = "otherStep")]
    public class OtherStepDto
    {
        public int OtherId { get; set; }
        public int StepId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int Amount { get; set; }
    }
}