using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microbrewit.Model
{
    public class FermentationStepFermentable
    {
        public int FermentableId { get; set; }
        public int StepNumber { get; set; }
        public int RecipeId { get; set; }
        public int Amount { get; set; }
        public double Lovibond { get; set; }
        public int PPG { get; set; }

        public FermentationStep FermentationStep { get; set; }
        public Fermentable Fermentable { get; set; }
    }
}
