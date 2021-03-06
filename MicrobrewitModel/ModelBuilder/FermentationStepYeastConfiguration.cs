﻿using System;
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
            Property(fsy => fsy.StepNumber).IsRequired();
            Property(fsy => fsy.YeastId).IsRequired();
            this.HasKey(fsy => new {fsy.StepNumber,fsy.YeastId, fsy.RecipeId});
        }
    }
}
