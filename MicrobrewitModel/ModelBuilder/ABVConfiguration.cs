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
            Property(a => a.AbvId).IsRequired().HasColumnName("AbvId").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            this.HasKey(a => a.AbvId);
            this.HasRequired(abv => abv.Beer).WithOptional(a => a.ABV);
        }
    }
}
