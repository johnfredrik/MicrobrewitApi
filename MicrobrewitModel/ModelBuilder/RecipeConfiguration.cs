using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microbrewit.Model.ModelBuilder
{
    public class RecipeConfiguration : EntityTypeConfiguration<Recipe>
    {
        public RecipeConfiguration()
        {
            Property(p => p.Id).IsRequired().HasColumnName("RecipeId");
            Property(p => p.Name).IsRequired();
            this.HasKey(r => r.Id);

            


            // relations
            this.HasRequired(r => r.Brewer).WithMany(user => user.Recipes).HasForeignKey(recipe => recipe.BrewerId);
            this.HasRequired(r => r.BeerStyle).WithMany().HasForeignKey(r => r.BeerStyleId);
            this.HasMany(r => r.MashSteps).WithMany(mashStep => mashStep.Recipes).Map(m =>
            {

                m.MapLeftKey("RecipeId");
                m.MapRightKey("MashStepId");
                m.ToTable("RecipeMashStep");
            });
            this.HasMany(r => r.BoilSteps).WithMany(boilStep => boilStep.Recipes).Map(m =>
            {

                m.MapLeftKey("RecipeId");
                m.MapRightKey("BoilStepId");
                m.ToTable("RecipeBoilStep");
            });
            this.HasMany(r => r.FermentationSteps).WithMany(fermentationStep => fermentationStep.Recipes).Map(m =>
                {
                    m.MapLeftKey("RecipeId");
                    m.MapRightKey("FermentationStepId");
                    m.ToTable("RecipeFermentationStep");
                });
        }
    }
}
