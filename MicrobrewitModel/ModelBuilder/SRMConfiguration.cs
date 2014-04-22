using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microbrewit.Model.ModelBuilder
{
    public class SRMConfiguration : EntityTypeConfiguration<SRM>
    {
        public SRMConfiguration()
        {
            Property(a => a.Id).IsRequired().HasColumnName("SrmId");
            this.HasKey(a => a.Id);
          //  this.HasOptional(a => a.Beer).WithOptionalDependent(b => b.SRM);
        }
    }
}
