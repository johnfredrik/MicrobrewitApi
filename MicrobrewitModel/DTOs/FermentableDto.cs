using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Microbrewit.Model.DTOs
{
    public class FermentableDto
    {
      
        public int Id { get; set; }
        public SupplierDto Supplier { get; set; }
        public string Name { get; set; }
        public double Lovibond { get; set; }
        public int PPG { get; set; }
        public string Type { get; set; }
        public string DataType { get { return "fermentable"; } }
        public bool Custom { get; set; }
      }

}