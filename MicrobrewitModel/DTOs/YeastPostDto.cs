using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microbrewit.Model.DTOs
{
    public class YeastPostDto
    {
      
        public int Id { get; set; }
        public string Name { get; set; }
        public double? TemperatureHigh { get; set; }
        public double? TemperatureLow { get; set; }
        public string Flocculation { get; set; }
        public string AlcoholTolerance { get; set; }
        public string ProductCode { get; set; }
        public string Comment { get; set; }
        public string Type { get; set; }
        public DTO Supplier { get; set; }
    }
}
