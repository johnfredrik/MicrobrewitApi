using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Microbrewit.Model.DTOs
{
    public class SRMDto
    {
        public int Id { get; set; }
        // Malt Calculate Units
        public int Standard { get; set; }
        public double Mosher { get; set; }
        public double Daniels { get; set; }
        public double Morey { get; set; }
    }
}