using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microbrewit.Model.ModelBuilder
{
    public class BreweryMemberConfiguration : EntityTypeConfiguration<BreweryMember>
    {
        public BreweryMemberConfiguration()
        {
            Property(b => b.MemberUsername).IsRequired();
            Property(b => b.BreweryId).IsRequired();
            this.HasKey(b => new { b.BreweryId, b.MemberUsername });
        }
    }
}
