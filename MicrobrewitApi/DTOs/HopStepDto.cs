using Microbrewit.Api.Automapper;
using Microbrewit.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Microbrewit.Api.DTOs
{
    public class HopStepDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double AAAmount { get; set; }
        public double AAValue { get; set; }
        public string Origin { get; set; }
        public string FlavourDescription { get; set; }
        public IList<FlavourDto> Flavours { get; set; }


    }
}