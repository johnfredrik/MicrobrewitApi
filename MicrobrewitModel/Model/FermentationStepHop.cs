using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microbrewit.Model
{

    public class FermentationStepHop
    {
        public int HopId { get; set; }
        public int StepNumber { get; set; }
        public int RecipeId { get; set; }
        public int AAValue { get; set; }
        public int AAAmount { get; set; }
        public int HopFormId { get; set; }

        public HopForm HopForm { get; private set; }
        public FermentationStep FermentationStep { get; set; }
        public Hop Hop { get; set; }
    }
}
