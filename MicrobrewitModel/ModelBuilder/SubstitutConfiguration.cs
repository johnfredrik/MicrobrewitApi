using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microbrewit.Model.ModelBuilder
{
    public class SubstituteConfiguration : EntityTypeConfiguration<Substitute>
    {
        public SubstituteConfiguration()
        {
            Property(sub => sub.SubstituteId).IsRequired();
            this.HasKey(sub => new {sub.SubstituteId, sub.HopId });
        }
    }

}
