using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microbrewit.Model
{
    public class FermentationStepYeast
    {
        public int YeastId { get; set; }
        public int FermentationStepId { get; set; }
        public int Amount { get; set; }

        public FermentationStep FermentationStep { get; set; }
        public Yeast Yeast { get; set; }
    }
}
