using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microbrewit.Model.ModelBuilder
{
    public class BoilStepOtherConfiguration : EntityTypeConfiguration<BoilStepOther>
    {
        public BoilStepOtherConfiguration()
        {
           
            Property(bso => bso.StepId).IsRequired();
            Property(bso => bso.OtherId).IsRequired();
            this.HasKey(bso => new {bso.StepId, bso.OtherId });

        }
    }
}
