using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microbrewit.Model
{
    public class IBU
    {
        public int IbuId { get; set; }
        public double Standard { get; set; }
        public double Tinseth { get; set; }
        public double Rager { get; set; }

        public Beer Beer { get; set; }

    }
}
