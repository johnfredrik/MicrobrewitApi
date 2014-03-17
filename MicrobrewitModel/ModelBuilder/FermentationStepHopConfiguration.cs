using Microbrewit.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microbrewit.Model.ModelBuilder
{
    public class FermentationStepHopConfiguration : EntityTypeConfiguration<FermentationStepHop>
    {
        public FermentationStepHopConfiguration()
        {
            Property(fsh => fsh.Id).IsRequired().HasColumnName("FermentationStepHopId");
            Property(fsh => fsh.HopId).IsRequired();
            Property(fsh => fsh.FermentationStepId).IsRequired();
            this.HasKey(rh => rh.Id);

        }
    }
}
