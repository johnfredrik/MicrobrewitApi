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
            Property(bsh => bsh.StepNumber).IsRequired();
            Property(bsh => bsh.RecipeId).IsRequired();
            this.HasKey(bsh => new {bsh.StepNumber,bsh.HopId, bsh.RecipeId});

            this.HasRequired(h => h.HopForm).WithMany().HasForeignKey(hopForm => hopForm.HopFormId);
        }
    }
}
