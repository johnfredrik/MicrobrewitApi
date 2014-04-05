using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microbrewit.Model;

namespace Microbrewit.Model
{
    public class Yeast
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double? TemperatureHigh { get; set; }
        public double? TemperatureLow { get; set; }
        public string Flocculation { get; set; }
        public string AlcoholTolerance { get; set; }
        public string ProductCode { get; set; }
        public string Comment { get; set; }
        [NotMapped]
        public string Type { get { return this.GetType().Name; } }

        public int? SupplierId { get; set; }
        public Supplier Supplier { get; set; }

        public ICollection<FermentationStepYeast> FermentationSteps { get; set; }

    }
}
