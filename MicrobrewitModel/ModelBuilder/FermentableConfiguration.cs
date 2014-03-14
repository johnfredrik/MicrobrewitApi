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
           

            Map(m =>
            {
                m.ToTable("Fermentable");
                m.Requires("Type").HasValue("");
            })
              .Map<Grain>(m =>
              {
                  m.Requires("Type").HasValue("Grain");
              })
              .Map<Sugar>(m =>
              {
                  m.Requires("Type").HasValue("Sugar");
              })
              .Map<LiquidExtract>(m =>
              {
                  m.Requires("Type").HasValue("Liquid Extract");
              })
              .Map<DryExtract>(m =>
              {
                  m.Requires("Type").HasValue("Dry Extract");
              });
         
            this.HasOptional(f => f.Supplier).WithMany().HasForeignKey(f => f.SupplierId);
            
            
        }
    }
}