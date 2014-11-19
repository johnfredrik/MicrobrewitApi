using Microbrewit.Model;
using Nest;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Microbrewit.Model.DTOs
{
    [ElasticType(Name = "yeast")]
    public class YeastDto
    {
        [JsonProperty(PropertyName = "yeastId")]
        public int Id { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "temperatureHigh")]
        public double? TemperatureHigh { get; set; }
        [JsonProperty(PropertyName = "temperatureLow")]
        public double? TemperatureLow { get; set; }
        [JsonProperty(PropertyName = "flocculation")]
        public string Flocculation { get; set; }
        [JsonProperty(PropertyName = "alcoholTolerance")]
        public string AlcoholTolerance { get; set; }
        [JsonProperty(PropertyName = "productCode")]
        public string ProductCode { get; set; }
        [JsonProperty(PropertyName = "notes")]
        public string Notes { get; set; }
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }
        [JsonProperty(PropertyName = "brewerySource")]
        public string BrewerySource { get; set; }
        [JsonProperty(PropertyName = "species")]
        public string Species { get; set; }
        [JsonProperty(PropertyName = "attenutionRange")]
        public string AttenutionRange { get; set; }
        [JsonProperty(PropertyName = "pitchingFermentationNotes")]
        public string PitchingFermentationNotes { get; set; }
        [JsonProperty(PropertyName = "supplier")]
        public DTO Supplier { get; set; }
        [JsonProperty(PropertyName = "dataType")]
        public string DataType { get { return "yeast"; } }
        [JsonProperty(PropertyName = "custom")]
        public bool Custom { get; set; }


    }
}