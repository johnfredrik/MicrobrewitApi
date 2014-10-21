using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microbrewit.Model
{
    public class Other
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public bool Custom { get; set; }

        public ICollection<MashStepOther> MashSteps { get; set; }
        public ICollection<BoilStepOther> BoilSteps { get; set; }
        public ICollection<FermentationStepOther> FermentationSteps { get; set; }

       

    }
}
