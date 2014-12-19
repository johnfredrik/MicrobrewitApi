using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Controllers;

namespace Microbrewit.Model.ModelBuilder
{
    public class FermentationStepConfiguration : EntityTypeConfiguration<FermentationStep>
    {
        public FermentationStepConfiguration()
        {
            //Property(f => f.Id).IsRequired().HasColumnName("FermentationStepId");
            //HasRequired(p => p.StepNumber).WithMany().HasForeignKey(p => new {p.StepNumber, p.RecipeId});
            HasKey(p => new {p.StepNumber, p.RecipeId});

            HasRequired(fs => fs.Recipe)  
            .WithMany(r => r.FermentationSteps) 
            .HasForeignKey(r => r.RecipeId);
            

            this.HasMany(f => f.Hops).WithRequired(fermentationStepHop => fermentationStepHop.FermentationStep).HasForeignKey(fermentationStepHop => new {fermentationStepHop.StepNumber, fermentationStepHop.RecipeId});
            this.HasMany(f => f.Fermentables).WithRequired(fermentationStepFermentable => fermentationStepFermentable.FermentationStep).HasForeignKey(fermentationStepFermentable => new { fermentationStepFermentable.StepNumber, fermentationStepFermentable.RecipeId });
            this.HasMany(f => f.Others).WithRequired(fermentationStepOther => fermentationStepOther.FermentationStep).HasForeignKey(fermentationStepOther => new { fermentationStepOther.StepNumber, fermentationStepOther.RecipeId });
            this.HasMany(f => f.Yeasts).WithRequired(fermentationStepYeast => fermentationStepYeast.FermentationStep).HasForeignKey(fermentationStepYeast => new { fermentationStepYeast.StepNumber, fermentationStepYeast.RecipeId });
        }
    }
}
