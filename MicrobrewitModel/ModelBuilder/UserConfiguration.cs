using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microbrewit.Model.ModelBuilder
{
    public class UserConfiguration : EntityTypeConfiguration<User>
    {
        public UserConfiguration()
        {
            //Property(u => u.Id).IsRequired().HasColumnName("UserId");
            Property(u => u.Username).IsRequired();
            Property(u => u.Email).IsRequired();

            //this.HasKey(u => u.Id);
            this.HasKey(u => u.Username);

                        
            this.HasMany(user => user.Breweries).WithRequired(breweryMember => breweryMember.Member).HasForeignKey(BreweryMember => BreweryMember.MemberId);
            this.HasMany(user => user.Beers).WithRequired(userBeer => userBeer.User).HasForeignKey(userBeer => userBeer.UserId);
        }
    }
}
