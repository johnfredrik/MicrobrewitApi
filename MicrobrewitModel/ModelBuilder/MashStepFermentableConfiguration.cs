using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microbrewit.Model.ModelBuilder
{
    public class MashStepFermentableConfiguration : EntityTypeConfiguration<MashStepFermentable>
    {
        public MashStepFermentableConfiguration()
        {
            Property(msf => msf.FermentableId).IsRequired();
            Property(msf => msf.StepId).IsRequired();
            this.HasKey(msf => new { msf.FermentableId, msf.StepId });
        }
    }
}
