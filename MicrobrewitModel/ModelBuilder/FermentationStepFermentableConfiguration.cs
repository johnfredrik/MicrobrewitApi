using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microbrewit.Model.ModelBuilder
{
    public class FermentationStepFermentableConfiguration : EntityTypeConfiguration<FermentationStepFermentable>
    {
        public FermentationStepFermentableConfiguration()
        {
            Property(fsf => fsf.FermentableId).IsRequired();
            Property(fsf => fsf.StepNumber).IsRequired();
            this.HasKey(fsf => new {fsf.FermentableId,fsf.StepNumber,fsf.RecipeId});
        }
    }
}
