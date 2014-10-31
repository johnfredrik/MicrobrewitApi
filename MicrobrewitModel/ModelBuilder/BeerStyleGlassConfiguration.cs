using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microbrewit.Model.ModelBuilder
{
    public class BeerStyleGlassConfiguration : EntityTypeConfiguration<BeerStyleGlass>
    {
        public BeerStyleGlassConfiguration()
        {
            Property(bsgc => bsgc.GlassId).IsRequired();
            Property(bsgc => bsgc.BeerStyleId).IsRequired();
            this.HasKey(bsgc => new { bsgc.BeerStyleId, bsgc.GlassId });
        }
    }
}
