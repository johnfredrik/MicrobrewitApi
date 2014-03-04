using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace MicrobrewitModel.ModelBuilder
{
    public class HopsConfiguration : EntityTypeConfiguration<Hop>
    {
        public HopsConfiguration()
        {
            Property(h => h.Name).IsRequired().HasMaxLength(200);    
        
            // relationships
            this.HasRequired(h => h.Origin).WithMany().HasForeignKey(o => o.OriginId);
        }
    }
}