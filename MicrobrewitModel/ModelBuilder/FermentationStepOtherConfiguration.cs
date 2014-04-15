using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microbrewit.Model.ModelBuilder
{
    public class FermentationStepOtherConfiguration : EntityTypeConfiguration<FermentationStepOther>
    {
        public FermentationStepOtherConfiguration()
        {
            Property(fso => fso.StepId).IsRequired();
            Property(fso => fso.OtherId).IsRequired();
            this.HasKey(fso => new {fso.StepId, fso.OtherId });

        }
    }
}
