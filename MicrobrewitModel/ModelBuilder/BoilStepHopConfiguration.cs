using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microbrewit.Model.ModelBuilder
{
    public class BoilStepHopConfiguration : EntityTypeConfiguration<BoilStepHop>
    {
        public BoilStepHopConfiguration()
        {
            Property(bsh => bsh.HopId).IsRequired();
            Property(bsh => bsh.StepId).IsRequired();
            this.HasKey(bsh => new {bsh.StepId,bsh.HopId});

            this.HasRequired(h => h.HopForm).WithMany().HasForeignKey(hopForm => hopForm.HopFormId);
        }
    }
}
