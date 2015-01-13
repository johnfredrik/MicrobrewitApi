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
            this.HasOptional(beer => beer.ABV).WithRequired(abv => abv.Beer).WillCascadeOnDelete(true);
            this.HasRequired(beer => beer.SRM).WithOptional(beer => beer.Beer).WillCascadeOnDelete(true);
            this.HasOptional(beer => beer.IBU).WithRequired(beer => beer.Beer).WillCascadeOnDelete(true);
            this.HasOptional(beer => beer.Recipe).WithRequired(recipe => recipe.Beer).WillCascadeOnDelete(true);

            this.HasMany(recipe => recipe.Forks)
    .WithOptional(recipe => recipe.ForkeOf)
    .HasForeignKey(recipe => recipe.ForkeOfId)
    .WillCascadeOnDelete(false);

        }
    }
}
