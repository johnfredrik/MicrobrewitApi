using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace Microbrewit.Model.ModelBuilder
{
    public class FermentableConfiguration : EntityTypeConfiguration<Fermentable>
    {
        public FermentableConfiguration()
        {
            Property(f => f.Id).IsRequired().HasColumnName("FermentableId");
            Property(f => f.Name).IsRequired().HasMaxLength(200);
            Property(f => f.Type).IsRequired().HasMaxLength(60);

            this.HasMany(fermentable => fermentable.BoilSteps).WithRequired(boilStepFermentable => boilStepFermentable.Fermentable).HasForeignKey(boilStepFermentable => boilStepFermentable.FermentableId);
            this.HasMany(fermentable => fermentable.FermentationSteps).WithRequired(fermentationStepFermentable => fermentationStepFermentable.Fermentable).HasForeignKey(fermentationStepFermentable => fermentationStepFermentable.FermentableId);
            this.HasOptional(f => f.Supplier).WithMany().HasForeignKey(f => f.SupplierId);
            
            HasMany(fermentable => fermentable.SubFermentables)
                .WithOptional(fermentable => fermentable.SuperFermentable)
                .HasForeignKey(fermentable => fermentable.SuperFermentableId)
                .WillCascadeOnDelete(false);

            

        }
    }
}