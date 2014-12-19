using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microbrewit.Model.ModelBuilder
{
    public class BoilStepConfiguration : EntityTypeConfiguration<BoilStep>
    {
        public BoilStepConfiguration()
        {
            HasKey(p => new {p.StepNumber, p.RecipeId});
            
            this.HasMany(b => b.Hops).WithRequired(boilStepHop => boilStepHop.BoilStep).HasForeignKey(boilStepHop => new {boilStepHop.StepNumber, boilStepHop.RecipeId});
            this.HasMany(b => b.Fermentables).WithRequired(boilStepFermentable => boilStepFermentable.BoilStep).HasForeignKey(boilStepFermentable => new {boilStepFermentable.StepNumber, boilStepFermentable.RecipeId});
            this.HasMany(b => b.Others).WithRequired(boilStepOthers => boilStepOthers.BoilStep).HasForeignKey(boilStepOthers => new {boilStepOthers.StepNumber, boilStepOthers.RecipeId});
            //this.HasMany(b => b.Recipe).WithRequired().HasForeignKey(b => b.RecipeId);
           
        }
    }
}
