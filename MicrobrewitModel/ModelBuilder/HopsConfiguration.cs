using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace MicrobrewitModel.ModelBuilder
{
    public class HopsConfiguration : EntityTypeConfiguration<Hop>
    {
        public HopsConfiguration()
        {
            this.HasKey(hop => hop.Id);
            Property(hop => hop.Id).IsRequired().HasColumnName("HopId").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(hop => hop.Name).IsRequired().HasMaxLength(200);
            this.HasMany(hop => hop.RecipeHops).WithRequired(recipeHop => recipeHop.Hop).HasForeignKey(recipeHop => recipeHop.HopId);
            //    .Map(m =>
            //{

            //    m.MapLeftKey("HopId");
            //    m.MapRightKey("RecipeId");
            //    m.ToTable("HopsRecipe");
            //});

            // relationships
            this.HasOptional(h => h.Origin).WithMany().HasForeignKey(o => o.OriginId);
        }
    }
}