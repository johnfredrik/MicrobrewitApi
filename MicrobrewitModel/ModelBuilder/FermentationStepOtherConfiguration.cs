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
            Property(fso => fso.StepNumber).IsRequired();
            Property(fso => fso.OtherId).IsRequired();
            this.HasKey(fso => new {fso.StepNumber, fso.OtherId,fso.RecipeId});
        }
    }
}
