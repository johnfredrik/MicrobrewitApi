using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microbrewit.Model;

namespace Microbrewit.Model
{
    public class Yeast
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double? TemperatureHigh { get; set; }
        public double? TemperatureLow { get; set; }
        public string Flocculation { get; set; }
        public string AlcoholTolerance { get; set; }
        public string ProductCode { get; set; }
        public string Notes { get; set; }
        public string Type { get; set; }
        // new from google doc.
        public string BrewerySource { get; set; }
        public string Species { get; set; }
        public string AttenutionRange { get; set; }
        public string PitchingFermentationNotes { get; set; }
       
        // relations
        public int? SupplierId { get; set; }
        public Supplier Supplier { get; set; }

        public ICollection<FermentationStepYeast> FermentationSteps { get; set; }

    }
}
