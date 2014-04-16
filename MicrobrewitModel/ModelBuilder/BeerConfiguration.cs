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
            this.HasMany(beer => beer.Breweries).WithRequired(breweryBeer => breweryBeer.Beer).HasForeignKey(breweryBeer => breweryBeer.BeerId);
           

        }
    }
}
