using MicrobrewitModel;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrobrewitModel.ModelBuilder
{
    public class RecipeHopConfiguration : EntityTypeConfiguration<RecipeHop>
    {
        public RecipeHopConfiguration()
        {
            Property(rh => rh.Id).IsRequired().HasColumnName("RecipeHopId");
            Property(rh => rh.HopId).IsRequired().HasColumnName("HopId");
            Property(rh => rh.RecipeId).IsRequired().HasColumnName("RecipeId");
            this.HasKey(rh => rh.Id);

        }
    }
}
