using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace Microbrewit.Model.ModelBuilder
{
    public class OriginConfiguration : EntityTypeConfiguration<Origin>
    {
        public OriginConfiguration()
        {
            
            Property(o => o.OriginId).IsRequired().HasColumnName("OriginId");
            Property(o => o.Name).IsRequired().HasMaxLength(255);

            
        }
    }
}