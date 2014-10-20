using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Microbrewit.Model.DTOs
{
    public class RecipeSimpleDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Notes { get; set; }
        public int Volume { get; set; }
        public DTO BeerStyle { get; set; }
        public string DataType { get { return "recipe"; } }

        //public IList<DTO> MashSteps { get; set; }
        //public IList<DTO> BoilSteps { get; set; }
        //public IList<FermentationStepDto> FermentationSteps { get; set; }
    }
}