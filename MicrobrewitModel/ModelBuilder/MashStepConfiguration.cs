using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microbrewit.Model.ModelBuilder
{
    public class MashStepConfiguration : EntityTypeConfiguration<MashStep>
    {
        public MashStepConfiguration()
        {
            Property(m => m.Id).HasColumnName("MashStepId").IsRequired();


            this.HasMany(m => m.Hops).WithRequired(mashStepHop => mashStepHop.MashStep).HasForeignKey(mashStepHop => mashStepHop.MashStepId);
            this.HasMany(m => m.Fermentables).WithRequired(mashStepFermentable => mashStepFermentable.MashStep).HasForeignKey(mashStepFermentable => mashStepFermentable.MashStepId);
            this.HasMany(m => m.Others).WithRequired(mashStepOther => mashStepOther.MashStep).HasForeignKey(mashStepOther => mashStepOther.MashStepId);
        }
    }
}
