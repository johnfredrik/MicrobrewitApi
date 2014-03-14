using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microbrewit.Model.ModelBuilder
{
    public class UserCredentialsConfiguration : EntityTypeConfiguration<UserCredentials>
    {
        public UserCredentialsConfiguration()
        {
            Property(u => u.Id).IsRequired().HasColumnName("UserCredentialsId");
            Property(u => u.Password).IsRequired();
            Property(u => u.SharedSecret).IsRequired();
            Property(u => u.Username).IsRequired().HasMaxLength(255);
          
            this.HasRequired(u => u.User).WithMany().HasForeignKey(u => u.Username);
        }
    }
}
