using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microbrewit.Model.ModelBuilder
{
    public class BreweryConfiguration : EntityTypeConfiguration<Brewery>
    {
        public BreweryConfiguration()
        {
            Property(b => b.Id).IsRequired().HasColumnName("BreweryId");
            Property(b => b.Name).IsRequired().HasMaxLength(255);

            this.HasMany(brewery => brewery.Members).WithRequired(breweryMember => breweryMember.Brewery).HasForeignKey(BreweryMember => BreweryMember.BreweryId);
           
        }
    }
}
