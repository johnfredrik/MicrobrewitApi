using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microbrewit.Model
{
    public class MashStep
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public int Temperature { get; set; }
        public string Type { get; set; }
        public int Length { get; set; }
        public int Volume { get; set; }
        public string Notes { get; set; }
        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; }

        public ICollection<MashStepFermentable> Fermentables { get; set; }
        public ICollection<MashStepHop> Hops { get; set; }
        public ICollection<MashStepOther> Others { get; set; }




    }
}
