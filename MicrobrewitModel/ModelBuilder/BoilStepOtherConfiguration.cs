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
            Property(bso => bso.Id).IsRequired().HasColumnName("BoilStepOtherId");
            Property(bso => bso.BoilStepId).IsRequired();
            Property(bso => bso.OtherId).IsRequired();
            this.HasKey(bso => bso.Id);

        }
    }
}
