using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microbrewit.Model
{
    public class ABV
    {
        public int Id { get; set; }
        public int Standard { get; set; }
        public int Formula1 { get; set; }
        public int Formula2 { get; set; }

        public Beer Beer { get; set; }

       
    }
}
