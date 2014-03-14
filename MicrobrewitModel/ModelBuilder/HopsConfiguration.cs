using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace Microbrewit.Model.ModelBuilder
{
    public class HopsConfiguration : EntityTypeConfiguration<Hop>
    {
        public HopsConfiguration()
        {
            this.HasKey(hop => hop.Id);
            Property(hop => hop.Id).IsRequired().HasColumnName("HopId").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(hop => hop.Name).IsRequired().HasMaxLength(200);
            this.HasMany(hop => hop.RecipeHops).WithRequired(recipeHop => recipeHop.Hop).HasForeignKey(recipeHop => recipeHop.HopId);
            this.HasMany(hop => hop.HopFlavours).WithRequired(flavourHop => flavourHop.Hop).HasForeignKey(flavourHop => flavourHop.HopId);
            this.HasMany(hop => hop.Substituts).WithMany();
            
            //this.HasMany(hop => hop.SubstituteHop).WithRequired(subHop => subHop.Hop).HasForeignKey(subHop => subHop.HopId);
            //this.HasMany(hop => hop.SubstituteHop).WithRequired(subHop => subHop.Substitute).HasForeignKey(subHop => subHop.SubstituteId);
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