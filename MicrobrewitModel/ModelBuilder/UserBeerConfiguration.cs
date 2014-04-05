using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microbrewit.Model.ModelBuilder
{
    public class UserBeerConfiguration : EntityTypeConfiguration<UserBeer>
    {
        public UserBeerConfiguration()
        {
            HasKey(userBeer => new { userBeer.BeerId, userBeer.UserId });
        }
    }
}
