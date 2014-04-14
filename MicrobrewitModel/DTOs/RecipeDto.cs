using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microbrewit.Model;
using System.Linq.Expressions;

namespace Microbrewit.Model.DTOs
{
    public class RecipeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string  Notes { get; set; }
        public int Volume { get; set; }
        public double OG { get; set; }
        public double FG { get; set; }
        public double Efficiency { get; set; }
        public DTO BeerStyle { get; set; }

        public IList<MashStepDto> MashSteps { get; set; }
        public IList<BoilStepDto> BoilSteps { get; set; }
        public IList<FermentationStepDto> FermentationSteps { get; set; }
    }
}