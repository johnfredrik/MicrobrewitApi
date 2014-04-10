using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microbrewit.Model
{
    public class BoilStepOther
    {
        public int OtherId { get; set; }
        public int StepId { get; set; }
        public int Amount { get; set; }

        public BoilStep BoilStep { get; set; }
        public Other Other { get; set; }
    }
}
