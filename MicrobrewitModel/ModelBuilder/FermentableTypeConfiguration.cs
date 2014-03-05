using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrobrewitModel.ModelBuilder
{
    class FermentableTypeConfiguration : EntityTypeConfiguration<FermentableType>
    {
        public FermentableTypeConfiguration()
        {
            Property(f => f.Id).IsRequired().HasColumnName("FermentableTypeId");
            Property(f => f.Name).IsRequired().HasMaxLength(200);
            
            this.HasKey(f => f.Id);
        }
    }
}  
