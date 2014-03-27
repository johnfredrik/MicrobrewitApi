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
       
        public int Id { get; set; }
        public int Volume { get; set; }
        public string Notes { get; set; }
        public int BeerStyleId { get; set; }
        public int? ForkeOfId { get; set; }
        // Single relations.
        public Recipe ForkeOf { get; set; }
        public Beer Beer { get; set; }

        //Multi relations
        public ICollection<Recipe> Forks { get; set; }
        public ICollection<MashStep> MashSteps { get; set; }
        public ICollection<BoilStep> BoilSteps { get; set; }
        public ICollection<FermentationStep> FermentationSteps { get; set; }
        

       
    }
}