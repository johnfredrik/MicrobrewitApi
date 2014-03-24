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

            //relation
            this.HasMany(m => m.Recipes).WithMany(r => r.Brewers).Map(m =>
            {
                m.MapLeftKey("Username");
                m.MapRightKey("RecipeId");
                m.ToTable("BrewerRecipe");
            });

            this.HasMany(user => user.Breweries).WithRequired(breweryMember => breweryMember.Member).HasForeignKey(BreweryMember => BreweryMember.MemberId);
        }
    }
}
