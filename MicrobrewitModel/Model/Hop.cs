using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Microbrewit.Model
{
    public class Hop
    {
    
        public int Id { get; set; }        
        public string Name { get; set; }
        public double AALow { get; set; }
        public double AAHigh { get; set; }
        public double BetaLow { get; set; }
        public double BetaHigh { get; set; }
        public string Notes { get; set; }
        public string FlavourDescription { get; set; }
        public int? OriginId { get; set; } 
        public Origin Origin { get; set; }

        public ICollection<HopFlavour> Flavours { get; set; }
        public ICollection<FermentationStepHop> FermentationSteps { get; set; }
        public ICollection<MashStepHop> MashSteps { get; set; }
        public ICollection<BoilStepHop> BoilSteps { get; set; }
        public ICollection<Hop> Substituts { get; set; }
    }
}