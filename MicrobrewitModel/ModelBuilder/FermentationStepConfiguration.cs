using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microbrewit.Model.ModelBuilder
{
    public class FermentationStepConfiguration : EntityTypeConfiguration<FermentationStep>
    {
        public FermentationStepConfiguration()
        {
            Property(f => f.Id).IsRequired().HasColumnName("FermentationStepId");


            this.HasMany(f => f.Hops).WithRequired(fermentationStepHop => fermentationStepHop.FermentationStep).HasForeignKey(fermentationStepHop => fermentationStepHop.FermentationStepId);
            this.HasMany(f => f.Fermentables).WithRequired(fermentationStepFermentable => fermentationStepFermentable.FermentationStep).HasForeignKey(fermentationStepFermentable => fermentationStepFermentable.FermentationStepId);
            this.HasMany(f => f.Others).WithRequired(fermentationStepOther => fermentationStepOther.FermentationStep).HasForeignKey(fermentationStepOther => fermentationStepOther.FermentationStepId);


            this.HasMany(f => f.Recipes).WithMany(recipe => recipe.FermentationSteps).Map(m =>
                {
                    m.MapLeftKey("FermentationStepId");
                    m.MapRightKey("RecipeId");
                    m.ToTable("RecipeFermentationStep");
                });
        }
    }
}
