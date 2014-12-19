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
           
            Property(bso => bso.StepNumber).IsRequired();
            Property(bso => bso.OtherId).IsRequired();
            Property(bso => bso.RecipeId).IsRequired();
            this.HasKey(bso => new {bso.StepNumber, bso.OtherId, bso.RecipeId});

        }
    }
}
