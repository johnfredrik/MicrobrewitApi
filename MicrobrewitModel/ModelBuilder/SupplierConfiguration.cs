using Microbrewit.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microbrewit.Model.ModelBuilder
{
    public class SupplierConfiguration : EntityTypeConfiguration<Supplier>
    {
        public SupplierConfiguration()
        {
            Property(s => s.Id).IsRequired().HasColumnName("SupplierId");
            Property(s => s.Name).IsRequired().HasMaxLength(255);

            this.HasRequired(s => s.Origin).WithMany().HasForeignKey(s => s.OriginId);
        }
    }
}
