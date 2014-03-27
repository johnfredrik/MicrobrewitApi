﻿using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microbrewit.Model.ModelBuilder
{
    public class MashStepOtherConfiguration : EntityTypeConfiguration<MashStepOther>
    {
        public MashStepOtherConfiguration()
        {
            Property(mso => mso.Id).IsRequired().HasColumnName("MashStepOtherId");
            Property(mso => mso.MashStepId).IsRequired();
            Property(mso => mso.OtherId).IsRequired();
            this.HasKey(mso => mso.Id);
        }
    }
}