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
    
        public int HopId { get; set; }        
        public string Name { get; set; }
        public double AALow { get; set; }
        public double AAHigh { get; set; }
        public double BetaLow { get; set; }
        public double BetaHigh { get; set; }
        public string Notes { get; set; }
        public double TotalOilHigh { get; set; }
        public double TotalOilLow { get; set; }
        public double LinaloolHigh { get; set; }
        public double LinaloolLow { get; set; }
        public double MyrceneHigh { get; set; }
        public double MyrceneLow { get; set; }
        public double CaryophylleneHigh { get; set; }
        public double CaryophylleneLow { get; set; }
        public double GeraniolHigh { get; set; }
        public double GeraniolLow { get; set; }
        public double FarneseneHigh { get; set; }
        public double FarneseneLow { get; set; }
        public double HumuleneHigh { get; set; }
        public double HumuleneLow { get; set; }
        public double OtherOilHigh { get; set; }
        public double OtherOilLow { get; set; }
        public double BPineneLow { get; set; }
        public double BPineneHigh { get; set; }
        public string Purpose { get; set; }
        public string Aliases { get; set; }
        public string FlavourDescription { get; set; }
        public bool Custom { get; set; }
        public int? OriginId { get; set; } 
        public Origin Origin { get; set; }

        public ICollection<HopFlavour> Flavours { get; set; }
        public ICollection<HopFlavour> AromaWheel { get; set; }
        public ICollection<FermentationStepHop> FermentationSteps { get; set; }
        public ICollection<MashStepHop> MashSteps { get; set; }
        public ICollection<BoilStepHop> BoilSteps { get; set; }
        public ICollection<Hop> Substituts { get; set; }
    }
}