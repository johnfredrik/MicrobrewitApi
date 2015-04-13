using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microbrewit.Model
{
    public class SpargeStepHop
    {
        public int HopId { get; set; }
        public int StepNumber { get; set; }
        public int RecipeId { get; set; }
        public int AaValue { get; set; }
        public int AaAmount { get; set; }
        public int HopFormId { get; set; }

        public HopForm HopForm { get; private set; }
        public SpargeStep SpargeStep { get; set; }
        public Hop Hop { get; set; }
    }
}
