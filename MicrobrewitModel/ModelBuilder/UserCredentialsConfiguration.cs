using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrobrewitModel.ModelBuilder
{
    class UserCredentialsConfiguration : EntityTypeConfiguration<UserCredentials>
    {
        UserCredentialsConfiguration()
        {
            Property(u => u.Id).IsRequired().HasColumnName("UserCredentialsId");
            Property(u => u.Password).IsRequired();
            Property(u => u.PasswordSalt).IsRequired();            
        }
    }
}
