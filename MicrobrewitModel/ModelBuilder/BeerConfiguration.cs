using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microbrewit.Model.ModelBuilder
{
    public class BeerConfiguration : EntityTypeConfiguration<Beer>
    {
        public BeerConfiguration()
        {
            Property(beer => beer.Id).IsRequired().HasColumnName("BeerId");
            Property(beer => beer.Name).IsRequired().HasMaxLength(255);
            this.HasKey(beer => beer.Id);
            this.HasOptional(beer => beer.BeerStyle).WithMany().HasForeignKey(beer => beer.BeerStyleId).WillCascadeOnDelete(false);
            this.HasMany(beer => beer.Brewers).WithRequired(userBeer => userBeer.Beer).HasForeignKey(userBeer => userBeer.BeerId);
            //this.HasMany(beer => beer.Brewers).WithMany(brewer => brewer.).Map(map =>
            //{
            //    map.MapLeftKey("BeerId");
            //    map.MapRightKey("BrewerId");
            //    map.ToTable("BrewerBeer");
            //});
            this.HasMany(beer => beer.Breweries).WithMany(brewery => brewery.Beers).Map(map =>
            {
                map.MapLeftKey("BeerId");
                map.MapRightKey("BreweryId");
                map.ToTable("BreweryBeer");
            });

        }
    }
}
