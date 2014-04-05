using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Microbrewit.Model.DTOs
{
    public class HopPostDto
    {
        public string Name { get; set; }
        public double AALow { get; set; }
        public double AAHigh { get; set; }
        public double BetaLow { get; set; }
        public double BetaHigh { get; set; }
        public string Notes { get; set; }
        public string FlavourDescription { get; set; }
        public DTO Origin { get; set; }

        public IList<DTO> Flavours { get; set; }
        public IList<DTO> Substituts { get; set; }
    }
}