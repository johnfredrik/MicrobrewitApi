using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microbrewit.Model.DTOs
{
    public class RecipePostDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Notes { get; set; }
        public DTO BeerStyle { get; set; }

        public IList<MashStepDto> MashSteps { get; set; }
        public IList<BoilStepDto> BoilSteps { get; set; }
        public IList<FermentationStepDto> FermentationSteps { get; set; }
    }
}
