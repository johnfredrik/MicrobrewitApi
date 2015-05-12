using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microbrewit.Model.ModelBuilder
{
    public class BreweryConfiguration : EntityTypeConfiguration<Brewery>
    {
        public BreweryConfiguration()
        {
            Property(b => b.BreweryId).IsRequired().HasColumnName("BreweryId");
            Property(b => b.Name).IsRequired().HasMaxLength(255).HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute("IX_BreweryName") {IsUnique = true}));

            HasRequired(s => s.Origin).WithMany().HasForeignKey(s => s.OriginId).WillCascadeOnDelete(false);
            HasMany(brewery => brewery.Members).WithRequired(breweryMember => breweryMember.Brewery).HasForeignKey(BreweryMember => BreweryMember.BreweryId);
            HasMany(brewery => brewery.Beers).WithRequired(breweryBeer => breweryBeer.Brewery).HasForeignKey(breweryBeer => breweryBeer.BreweryId);
            HasMany(brewery => brewery.Socials)
                .WithRequired(socials => socials.Brewery)
                .HasForeignKey(socials => socials.BreweryId);
        }
    }
}
