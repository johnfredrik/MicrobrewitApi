using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microbrewit.Model.ModelBuilder
{
    public class BoilStepFermentableConfiguration : EntityTypeConfiguration<BoilStepFermentable>
    {
        public BoilStepFermentableConfiguration()
        {
            Property(bsf => bsf.Id).IsRequired().HasColumnName("BoilStepFermentableId");
            Property(bsf => bsf.FermentableId).IsRequired();
            Property(bsf => bsf.BoilStepId).IsRequired();
            this.HasKey(bsf => bsf.Id);
        }
    }
}
