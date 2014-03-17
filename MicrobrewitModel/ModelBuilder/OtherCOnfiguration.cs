using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microbrewit.Model.ModelBuilder
{
    class OtherConfiguration : EntityTypeConfiguration<Other>
    {
        public OtherConfiguration()
        {
            Property(o => o.Id).IsRequired().HasColumnName("OtherId");
            Property(o => o.Name).IsRequired().HasMaxLength(255);
            this.HasKey(o => o.Id);


            Map(m =>
            {
                m.ToTable("Other");
                m.Requires("Type").HasValue("");
            })
              .Map<Fruit>(m =>
              {
                  m.Requires("Type").HasValue("Fruit");
              })
              .Map<Spice>(m =>
              {
                  m.Requires("Type").HasValue("Spice");
              })
              .Map<NoneFermentableSugar>(m => {
                  m.Requires("Type").HasValue("NoneFermentableSugar");
              });
        
            this.HasMany(other => other.MashSteps).WithRequired(mashStepOther => mashStepOther.Other).HasForeignKey(mashStepOther => mashStepOther.OtherId);
            this.HasMany(other => other.BoilSteps).WithRequired(boilStepOther => boilStepOther.Other).HasForeignKey(boilStepOther => boilStepOther.OtherId);
            this.HasMany(other => other.FermentationSteps).WithRequired(fermentationStepOther => fermentationStepOther.Other).HasForeignKey(fermentationStepOther => fermentationStepOther.OtherId);


        }
    }
}
