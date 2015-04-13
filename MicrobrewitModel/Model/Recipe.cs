using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Microbrewit.Model
{
    public class Recipe
    {

        public int RecipeId { get; set; }
        public int Volume { get; set; }
        public string Notes { get; set; }
        public int? ForkeOfId { get; set; }
        public double OG { get; set; }
        public double FG { get; set; }
        public double Efficiency { get; set; }
        public int TotalBoilTime { get; set; }
        // Single relations.
        public Beer Beer { get; set; }
        public SpargeStep SpargeStep { get; set; }
        //Multi relations
        public ICollection<MashStep> MashSteps { get; set; }
        public ICollection<BoilStep> BoilSteps { get; set; }
        public ICollection<FermentationStep> FermentationSteps { get; set; }
        

       
    }
}