using Microbrewit.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microbrewit.Model.ModelBuilder
{
    public class YeastConfiguration : EntityTypeConfiguration<Yeast>
    {
        public YeastConfiguration()
        {
            Property(y => y.YeastId).IsRequired().HasColumnName("YeastId");
            Property(y => y.Name).IsRequired().HasMaxLength(255);        

            this.HasOptional(y => y.Supplier).WithMany().HasForeignKey(y => y.SupplierId);
            this.HasMany(yeast => yeast.FermentationSteps).WithRequired(FermentationStepYeast => FermentationStepYeast.Yeast).HasForeignKey(FermentationStepYeast => FermentationStepYeast.YeastId);

        }
    }
}
