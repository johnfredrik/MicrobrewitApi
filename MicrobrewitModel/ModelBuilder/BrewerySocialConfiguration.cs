using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microbrewit.Model.ModelBuilder
{
    public class BrewerySocialConfiguration : EntityTypeConfiguration<BrewerySocial>
    {
        public BrewerySocialConfiguration()
        {
            Property(b => b.SocialId).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            HasKey(b => new {b.BreweryId, b.SocialId});

        }
    }
}
