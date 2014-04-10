using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microbrewit.Model.ModelBuilder
{
    public class MashStepHopConfiguration : EntityTypeConfiguration<MashStepHop>
    {
        public MashStepHopConfiguration()
        {
            Property(msh => msh.HopId).IsRequired();
            Property(msh => msh.MashStepId).IsRequired();
            this.HasKey(msh => new { msh.HopId,msh.MashStepId});
        }
    }
}
