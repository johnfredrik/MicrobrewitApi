using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microbrewit.Model.ModelBuilder
{
    public class MashStepOtherConfiguration : EntityTypeConfiguration<MashStepOther>
    {
        public MashStepOtherConfiguration()
        {
            Property(mso => mso.StepNumber).IsRequired();
            Property(mso => mso.OtherId).IsRequired();
            this.HasKey(mso => new {mso.StepNumber,mso.OtherId, mso.RecipeId });
        }
    }
}
