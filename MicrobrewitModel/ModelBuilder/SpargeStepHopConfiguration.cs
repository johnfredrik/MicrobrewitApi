using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microbrewit.Model.ModelBuilder
{
    public class SpargeStepHopConfiguration : EntityTypeConfiguration<SpargeStepHop>
    {
        public SpargeStepHopConfiguration()
        {
            Property(msh => msh.HopId).IsRequired();
            Property(msh => msh.StepNumber).IsRequired();
            this.HasKey(msh => new { msh.HopId,msh.StepNumber, msh.RecipeId});
        }
    }
}
