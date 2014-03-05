using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrobrewitModel.ModelBuilder
{
    public class UserConfiguration : EntityTypeConfiguration<User>
    {
        UserConfiguration()
        {
            Property(u => u.Id).IsRequired().HasColumnName("UserId");
            Property(u => u.Username).IsRequired();
            Property(u => u.Email).IsRequired();

            this.HasRequired(u => u.UserCredentials).WithMany().HasForeignKey(u => u.UserCredentialsId);
        }
    }
}
