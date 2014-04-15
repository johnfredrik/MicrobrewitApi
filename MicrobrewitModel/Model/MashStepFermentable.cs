using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microbrewit.Model
{
    public class MashStepFermentable
    {
        public int FermentableId { get; set; }
        public int StepId { get; set; }
        public int Amount { get; set; }
        public double Lovibond { get; set; }

        public MashStep MashStep { get; set; }
        public Fermentable Fermentable { get; set; }
    }
}
