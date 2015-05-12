using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microbrewit.Model.ModelBuilder
{
    public class GlassConfiguration : EntityTypeConfiguration<Glass>
    {
        public GlassConfiguration()
        {
            Property(a => a.GlassId).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            this.HasKey(a => a.GlassId);

            this.HasMany(glass => glass.BeerStyles).WithRequired(beerStyleGlass => beerStyleGlass.Glass).HasForeignKey(beerStyleGlass => beerStyleGlass.GlassId);
        }
    }
}
