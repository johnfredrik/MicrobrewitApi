using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace MicrobrewitModel.ModelBuilder
{
    public class OriginConfiguration : EntityTypeConfiguration<Origin>
    {
        public OriginConfiguration()
        {
            
            Property(o => o.Id).IsRequired().HasColumnName("OriginId").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(o => o.Name).IsRequired().HasMaxLength(255);

            
        }
    }
}