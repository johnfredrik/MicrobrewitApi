using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrobrewitModel
{
    public class Yeast
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int TemperatureHigh { get; set; }
        public int TemperatureLow { get; set; }
        public string Flocculation { get; set; }
        public string AlcoholTolerance { get; set; }
        public string ProductCode { get; set; }
        public string Comment { get; set; }

        public Nullable<int> SupplierId { get; set; }
        public Supplier Supplier { get; set; }

    }
}
