﻿using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microbrewit.Model.ModelBuilder
{
    public class HopFormConfiguration : EntityTypeConfiguration<HopForm>
    {
        public HopFormConfiguration()
        {
            Property(form => form.Id).IsRequired();
            Property(form => form.Name).IsRequired().HasMaxLength(60);


        }
    }
}
