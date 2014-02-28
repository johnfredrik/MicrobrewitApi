using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace MicrobrewitApi.Models.ModelBuilder
{
    public class OriginConfiguration : EntityTypeConfiguration<Origin>
    {
        public OriginConfiguration()
        {
            Property(o => o.Name).IsRequired().HasMaxLength(255);
        }
    }
}