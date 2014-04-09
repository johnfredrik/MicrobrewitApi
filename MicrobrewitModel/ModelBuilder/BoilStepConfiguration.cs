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
            Property(b => b.Id).HasColumnName("BoilStepId").IsRequired();
            
            this.HasMany(b => b.Hops).WithRequired(boilStepHop => boilStepHop.BoilStep).HasForeignKey(boilStepHop => boilStepHop.StepId);
            this.HasMany(b => b.Fermentables).WithRequired(boilStepFermentable => boilStepFermentable.BoilStep).HasForeignKey(boilStepFermentable => boilStepFermentable.StepId);
            this.HasMany(b => b.Others).WithRequired(boilStepOthers => boilStepOthers.BoilStep).HasForeignKey(boilStepOthers => boilStepOthers.StepId);
            //this.HasMany(b => b.Recipe).WithRequired().HasForeignKey(b => b.RecipeId);
           
        }
    }
}
