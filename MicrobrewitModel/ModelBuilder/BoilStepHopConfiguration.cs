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
            Property(bsh => bsh.Id).IsRequired().HasColumnName("BoilStepHopId");
            Property(bsh => bsh.HopId).IsRequired();
            Property(bsh => bsh.BoilStepId).IsRequired();
            this.HasKey(bsh => bsh.Id);
        }
    }
}
