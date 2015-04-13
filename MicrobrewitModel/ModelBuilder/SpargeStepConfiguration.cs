using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microbrewit.Model.ModelBuilder
{
    public class SpargeStepConfiguration : EntityTypeConfiguration<SpargeStep>
    {
        public SpargeStepConfiguration()
        {
            HasKey(m => m.RecipeId);
            //HasKey(m => m.StepNumber);

            this.HasMany(spargeStep => spargeStep.Hops).WithRequired(spargeStepHop => spargeStepHop.SpargeStep).HasForeignKey(spargeStepHop => spargeStepHop.RecipeId);
        }
    }
}
