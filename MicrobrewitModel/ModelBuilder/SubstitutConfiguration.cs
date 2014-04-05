using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microbrewit.Model.ModelBuilder
{
    
        class SubstitutConfiguration : EntityTypeConfiguration<Substitut>
        {
            public SubstitutConfiguration()
            {
                this.HasKey(sub => new { sub.SubstitutId, sub.HopId });

            }
        }

}
