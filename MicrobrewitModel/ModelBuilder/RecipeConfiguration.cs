using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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
            Property(p => p.Id).IsRequired().HasColumnName("RecipeId").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            this.HasRequired(r => r.Beer).WithRequiredDependent(r => r.Recipe);

            this.HasKey(r => r.Id);
            
            // relations
            this.HasMany(r => r.BoilSteps).WithRequired().HasForeignKey(boilStep => boilStep.RecipeId);
            this.HasMany(r => r.FermentationSteps).WithRequired().HasForeignKey(fermentationStep => fermentationStep.RecipeId);
            this.HasMany(r => r.MashSteps).WithRequired().HasForeignKey(mashStep => mashStep.RecipeId);
           

            this.HasMany(recipe => recipe.Forks)
                .WithOptional(recipe => recipe.ForkeOf)
                .HasForeignKey(recipe => recipe.ForkeOfId)
                .WillCascadeOnDelete(false);

            this.HasMany(recipe => recipe.Forks).WithMany().Map(map =>
                {
                    map.MapLeftKey("RecipeId");
                    map.MapRightKey("ForkedRecipeId");
                    map.ToTable("ForkedRecipe");
                });
        }
    }
}
