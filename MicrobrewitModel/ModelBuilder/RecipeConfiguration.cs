using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microbrewit.Model.ModelBuilder
{
    public class RecipeConfiguration : EntityTypeConfiguration<Recipe>
    {
        public RecipeConfiguration()
        {
            Property(p => p.RecipeId).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            this.HasKey(r => r.RecipeId);
            //this.HasRequired(r => r.Beer).WithRequiredDependent(r => r.Recipe);
            HasRequired(r => r.Beer).WithOptional();
            HasOptional(r => r.SpargeStep).WithRequired(s => s.Recipe).WillCascadeOnDelete(true);
            //HasOptional(r => r.SpargeStep).WithMany().HasForeignKey(s => s.RecipeId);
            // relations
            this.HasMany(r => r.BoilSteps).WithRequired().HasForeignKey(boilStep => boilStep.RecipeId);            
            this.HasMany(r => r.FermentationSteps).WithRequired().HasForeignKey(fermentationStep => fermentationStep.RecipeId);
            this.HasMany(r => r.MashSteps).WithRequired().HasForeignKey(mashStep => mashStep.RecipeId);
            //this.HasRequired(r => r.SpargeStep).WithMany().HasForeignKey(spargeStep => spargeStep.RecipeId);




          
        }
    }
}
