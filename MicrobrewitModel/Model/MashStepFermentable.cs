using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microbrewit.Model
{
    public class MashStepFermentable
    {
        public int Id { get; set; }
        public int FermentableId { get; set; }
        public int MashStepId { get; set; }
        public int Amount { get; set; }

        public MashStep MashStep { get; set; }
        public Fermentable Fermentable { get; set; }
    }
}
