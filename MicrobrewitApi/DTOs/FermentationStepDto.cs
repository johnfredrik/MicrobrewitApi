using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Microbrewit.Api.DTOs
{
    public class FermentationStepDto
    {       
            public int Number { get; set; }
            public string Type { get; set; }
            public int Length { get; set; }
            public int Volume { get; set; }
            public int Temperature { get; set; }
            public string Notes { get; set; }
            public IList<HopStepDto> Hops { get; set; }
            public IList<FermentableStepDto> Fermentables { get; set; }
            public IList<OtherStepDto> Others { get; set; }
            public IList<YeastStepDto> Yeasts { get; set; }
    }
}