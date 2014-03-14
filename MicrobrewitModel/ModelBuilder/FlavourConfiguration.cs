using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microbrewit.Model.ModelBuilder
{
    class FlavourConfiguration : EntityTypeConfiguration<Flavour>
    {
        public FlavourConfiguration()
        {
            Property(f => f.Id).HasColumnName("FlavourId").IsRequired();
            Property(f => f.Name).IsRequired().HasMaxLength(255);

            this.HasMany(flavour => flavour.Hops).WithRequired(flavourHop => flavourHop.Flavour).HasForeignKey(flavourHop => flavourHop.FlavourId);
                

        }

    }
}
