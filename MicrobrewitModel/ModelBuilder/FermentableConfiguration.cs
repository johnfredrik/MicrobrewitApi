using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace MicrobrewitModel.ModelBuilder
{
    public class FermentableConfiguration : EntityTypeConfiguration<Fermentable>
    {
        public FermentableConfiguration()
        {
            Property(f => f.Id).IsRequired().HasColumnName("FermentableId");
            Property(f => f.Name).IsRequired().HasMaxLength(200);
            Property(f => f.TypeId).IsRequired().HasColumnName("FermentableTypeId");

            this.HasRequired(f => f.Type).WithMany().HasForeignKey( f => f.TypeId);
            this.HasOptional(f => f.Supplier).WithMany().HasForeignKey(f => f.SupplierId);
            
            
        }
    }
}