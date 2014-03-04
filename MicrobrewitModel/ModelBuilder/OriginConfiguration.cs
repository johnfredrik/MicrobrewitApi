using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace MicrobrewitModel.ModelBuilder
{
    public class OriginConfiguration : EntityTypeConfiguration<Origin>
    {
        public OriginConfiguration()
        {
            this.HasKey(o => o.OriginId);
            Property(o => o.Name).IsRequired().HasMaxLength(255);
        }
    }
}