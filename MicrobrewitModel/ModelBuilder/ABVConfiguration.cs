using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microbrewit.Model.ModelBuilder
{
    public class ABVConfiguration : EntityTypeConfiguration<ABV>
    {
        public ABVConfiguration()
        {
            Property(a => a.Id).IsRequired().HasColumnName("AbvId");
            this.HasKey(a => a.Id);
            this.HasOptional(a => a.Beer).WithOptionalDependent(b => b.ABV);
        }
    }
}
