using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microbrewit.Model
{
    public class BoilStep
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public int Length { get; set; }
        public int Volume { get; set; }
        public string Notes { get; set; }
        public int RecipeId { get; set; }

        public Recipe Recipe { get; set; }
        public ICollection<BoilStepHop> Hops { get; set; }
        public ICollection<BoilStepFermentable> Fermentables { get; set; }
        public ICollection<BoilStepOther> Others { get; set; }

    }
}
