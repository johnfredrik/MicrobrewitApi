using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microbrewit.Model;
using System.Linq.Expressions;

namespace Microbrewit.Api.DTOs
{
    public class RecipeDto
    {
        public int Id { get; set; }
        public string RecipeName { get; set; }
        public string  Notes { get; set; }

        public IList<BrewerDto> Brewers { get; set; }
        public IList<MashStepDto> MashSteps { get; set; }
        public IList<BoilStepDto> BoilSteps { get; set; }
        public IList<FermentationStepDto> FermentationSteps { get; set; }
    }
}