using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microbrewit.Model.ModelBuilder
{
    public class IBUConfiguration : EntityTypeConfiguration<IBU>
    {
        public IBUConfiguration()
        {
            Property(a => a.IbuId).IsRequired().HasColumnName("IbuId").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            this.HasKey(a => a.IbuId);
          //  this.HasOptional(a => a.Beer).WithOptionalDependent(b => b.IBU);
        }
    }
}
