using Microbrewit.Api.Automapper;
using Microbrewit.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Microbrewit.Api.DTOs
{
    public class HopDto
    {
        public string Name { get; set; }
        public double AALow { get; set; }
        public double AAHigh { get; set; }
        public string Origin { get; set; }
        public string FlavourDescription { get; set; }
        public IList<FlavourDto> Flavours { get; set; }


    }
}