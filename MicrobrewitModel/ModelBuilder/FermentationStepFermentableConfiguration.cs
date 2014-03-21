using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microbrewit.Model.ModelBuilder
{
    public class FermentationStepFermentableConfiguration : EntityTypeConfiguration<FermentationStepFermentable>
    {
        public FermentationStepFermentableConfiguration()
        {
            Property(fsf => fsf.Id).IsRequired().HasColumnName("FermentationStepFermentableId");
            Property(fsf => fsf.FermentableId).IsRequired();
            Property(fsf => fsf.FermentationStepId).IsRequired();
            this.HasKey(fsf => fsf.Id);
        }
    }
}
