using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace MicrobrewitApi.Models.ModelBuilder
{
    public class FermentableConfiguration : EntityTypeConfiguration<Fermentable>
    {
        public FermentableConfiguration()
        {
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
            .Map<LiquidExtract>(m =>
            {
                m.Requires("Type").HasValue("Liquid Extract");
            });

            

        }
    }
}