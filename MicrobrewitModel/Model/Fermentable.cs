using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Microbrewit.Model
{
    public class Fermentable
    {
        
        public int Id { get; set; }
        public string Name { get; set; }
        public double EBC { get; set; }
        public double Lovibond { get; set; }
        public int? PPG { get; set; }
        public int? SupplierId { get; set; }
        public string Type { get; set; }
        public bool Custom { get; set; }

        public Supplier Supplier { get; set; }
        public ICollection<MashStepFermentable> MashSteps { get; set; }
        public ICollection<BoilStepFermentable> BoilSteps { get; set; }
        public ICollection<FermentationStepFermentable> FermentationSteps { get; set; }
    }
}