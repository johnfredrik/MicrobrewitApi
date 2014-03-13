using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrobrewitModel.ModelBuilder
{
    public class RecipeConfiguration : EntityTypeConfiguration<Recipe>
    {
        public RecipeConfiguration()
        {
            Property(p => p.Id).IsRequired().HasColumnName("RecipeId");
            Property(p => p.Name).IsRequired();


            this.HasKey(r => r.Id);
            this.HasMany(recipe => recipe.RecipeHops).WithRequired(recipeHop => recipeHop.Recipe).HasForeignKey(recipeHop => recipeHop.RecipeId);

            //this.HasMany(r => r.Hops).WithMany(h => h.Recipes).Map(m =>
            //{

            //    m.MapLeftKey("RecipeId");
            //    m.MapRightKey("HopId");
            //    m.ToTable("HopsRecipe");
            //});
        }
    }
}
