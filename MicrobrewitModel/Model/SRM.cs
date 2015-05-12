using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microbrewit.Model
{
    public class SRM
    {
        public int SrmId { get; set; }
        public double Standard { get; set; }
        public double Mosher { get; set; }
        public double Daniels { get; set; }
        public double Morey { get; set; }

        public Beer Beer { get; set; }
    }
}
