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
            Property(a => a.SrmId).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            this.HasKey(a => a.SrmId);
          //  this.HasOptional(a => a.Beer).WithOptionalDependent(b => b.SRM);
        }
    }
}
