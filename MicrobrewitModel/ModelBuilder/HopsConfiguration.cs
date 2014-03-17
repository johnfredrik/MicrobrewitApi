using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace Microbrewit.Model.ModelBuilder
{
    public class HopsConfiguration : EntityTypeConfiguration<Hop>
    {
        public HopsConfiguration()
        {
            this.HasKey(hop => hop.Id);
            Property(hop => hop.Id).IsRequired().HasColumnName("HopId").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(hop => hop.Name).IsRequired().HasMaxLength(200);

            // relationships
            this.HasMany(hop => hop.FermentationSteps).WithRequired(fermentationStepHop => fermentationStepHop.Hop).HasForeignKey(fermentationStepHop => fermentationStepHop.HopId);
            this.HasMany(hop => hop.MashSteps).WithRequired(mashStepHop => mashStepHop.Hop).HasForeignKey(mashStepHop => mashStepHop.HopId);
            this.HasMany(hop => hop.HopFlavours).WithRequired(flavourHop => flavourHop.Hop).HasForeignKey(flavourHop => flavourHop.HopId);
            this.HasMany(hop => hop.Substituts).WithMany();
            this.HasOptional(h => h.Origin).WithMany().HasForeignKey(o => o.OriginId);
        }
    }
}