using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microbrewit.Model
{
    public class SpargeStep
    {
        public int StepNumber { get; set; }
        public int Temperature { get; set; }
        public int Amount { get; set; }
        public string Notes { get; set; }
        public string Type { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; }
        public ICollection<SpargeStepHop> Hops { get; set; }
    }
}
