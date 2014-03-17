using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microbrewit.Model
{
    public class MashStepOther
    {
        public int Id { get; set; }
        public int OtherId { get; set; }
        public int MashStepId { get; set; }
        public int Amount { get; set; }

        public MashStep MashStep { get; set; }
        public Other Other { get; set; }
    }
}
