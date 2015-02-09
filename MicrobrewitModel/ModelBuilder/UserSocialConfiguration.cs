using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microbrewit.Model.ModelBuilder
{
    public class UserSocialConfiguration : EntityTypeConfiguration<UserSocial>
    {
        public UserSocialConfiguration()
        {
            Property(b => b.SocialId).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            HasKey(b => new {b.Username, b.SocialId});
        }
    }
}
