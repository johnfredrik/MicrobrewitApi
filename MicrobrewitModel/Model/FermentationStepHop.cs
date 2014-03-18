using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microbrewit.Model
{

    public class FermentationStepHop
    {
        public int Id { get; set; }
        public int HopId { get; set; }
        public int FermentationStepId { get; set; }
        public int AAValue { get; set; }
        public int AAAmount { get; set; }


        public FermentationStep FermentationStep { get; set; }
        public Hop Hop { get; set; }
    }
}
