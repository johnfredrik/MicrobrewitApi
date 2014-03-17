using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microbrewit.Model.ModelBuilder
{
    public class FermentationStepYeastConfiguration : EntityTypeConfiguration<FermentationStepYeast>
    {
        public FermentationStepYeastConfiguration()
        {
            Property(fsy => fsy.Id).IsRequired().HasColumnName("FermentationStepYeastId");
            Property(fsy => fsy.FermentationStepId).IsRequired();
            Property(fsy => fsy.YeastId).IsRequired();
            this.HasKey(fsy => fsy.Id);
        }
    }
}
