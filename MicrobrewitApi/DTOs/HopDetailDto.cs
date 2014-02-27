using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MicrobrewitApi.DTOs
{
    public class HopDetailDto
    {
        public string Name { get; set; }
        public double AALow { get; set; }
        public double AAHigh { get; set; }
        public string Origin { get; set; }

    }
}