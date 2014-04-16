using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microbrewit.Model.ModelBuilder
{
    public class BreweryBeerConfiguration : EntityTypeConfiguration<BreweryBeer>
    {
        public BreweryBeerConfiguration()
        {
            HasKey(breweryBeer => new {breweryBeer.BeerId, breweryBeer.BreweryId});
        }
    }
}
