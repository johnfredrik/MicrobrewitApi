using Microbrewit.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Microbrewit.Model.DTOs
{
    public class YeastDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double? TemperatureHigh { get; set; }
        public double? TemperatureLow { get; set; }
        public string Flocculation { get; set; }
        public string AlcoholTolerance { get; set; }
        public string ProductCode { get; set; }
        public string Notes { get; set; }
        public string Type { get; set; }
        public string BrewerySource { get; set; }
        public string Species { get; set; }
        public string AttenutionRange { get; set; }
        public string PitchingFermentationNotes { get; set; }      
        public DTO Supplier { get; set; }
        public string DataType { get { return "yeast"; } }


    }
}