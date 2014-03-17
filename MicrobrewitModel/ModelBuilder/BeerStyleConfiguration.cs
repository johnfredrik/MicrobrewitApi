using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microbrewit.Model.ModelBuilder
{
    public class BeerStyleConfiguration : EntityTypeConfiguration<BeerStyle>
    {
        public BeerStyleConfiguration()
        {
            Property(b => b.Id).IsRequired().HasColumnName("BeerStyleId");
            Property(b => b.Name).IsRequired().HasMaxLength(255);
            this.HasKey(beerStyle => beerStyle.Id);

            // relations
            this.HasMany(beerStyle => beerStyle.SubStyles)
            .WithOptional(beerStyle => beerStyle.SuperStyle)
            .HasForeignKey(beerStyle => beerStyle.SuperStyleId)
            .WillCascadeOnDelete(false);

            this.HasMany(beerStyle => beerStyle.Recipes).WithRequired(recipe => recipe.BeerStyle).HasForeignKey(recipe => recipe.BeerStyleId);
           

        }
    }
}
