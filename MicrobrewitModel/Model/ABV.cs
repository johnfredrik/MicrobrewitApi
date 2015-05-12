using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microbrewit.Model
{
    public class ABV
    {
        public int AbvId { get; set; }
        public double Standard { get; set; }
        public double Miller { get; set; }
        public double Advanced { get; set; }
        public double AdvancedAlternative { get; set; }
        public double Simple { get; set; }
        public double AlternativeSimple { get; set; }
       
        public Beer Beer { get; set; }

       
    }
}
