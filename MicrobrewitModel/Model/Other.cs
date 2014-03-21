﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microbrewit.Model
{
    public class Other
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [NotMapped]
        public string Type { get { return this.GetType().Name; } }

    }
}