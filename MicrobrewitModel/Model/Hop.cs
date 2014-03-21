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
        public string FlavourDescription { get; set; }
        public Nullable<int> OriginId { get; set; } 
        public virtual Origin Origin { get; set; }

        public ICollection<HopFlavour> HopFlavours { get; set; }
        public ICollection<FermentationStepHop> FermentationSteps { get; set; }
        public ICollection<MashStepHop> MashSteps { get; set; }
        public ICollection<BoilStepHop> BoilSteps { get; set; }
        public ICollection<Hop> Substituts { get; set; }
    }
}