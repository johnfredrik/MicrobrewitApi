using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Microbrewit.Model.DTOs
{
    [ElasticType(Name = "yeast")]
    public class YeastStepDto
    {
        public int YeastId { get; set; }
        public int StepId { get; set; }
        public string Name { get; set; }
        public int Amount { get; set; }
        public string Type { get; set; }
        public DTO Supplier { get; set; }
    }
}