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
            Property(y => y.Id).IsRequired().HasColumnName("YeastId");
            Property(y => y.Name).IsRequired().HasMaxLength(255);        

            this.HasRequired(y => y.Supplier).WithMany().HasForeignKey(y => y.SupplierId);
            this.HasMany(yeast => yeast.FermentationSteps).WithRequired(FermentationStepYeast => FermentationStepYeast.Yeast).HasForeignKey(FermentationStepYeast => FermentationStepYeast.YeastId);

            Map(m =>
            {
                m.ToTable("Yeast");
                m.Requires("Type").HasValue("");
            })
            .Map<LiquidYeast>(m =>
            {
                m.Requires("Type").HasValue("Liquid Yeast");
            })
            .Map<DryYeast>(m =>
            {
                m.Requires("Type").HasValue("Dry Yeast");
            });
        }
    }
}
