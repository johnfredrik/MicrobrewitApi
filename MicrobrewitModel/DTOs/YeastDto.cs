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
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "temperaturehigh")]
        public double? TemperatureHigh { get; set; }
        [JsonProperty(PropertyName = "temperaturelow")]
        public double? TemperatureLow { get; set; }
        [JsonProperty(PropertyName = "flocculation")]
        public string Flocculation { get; set; }
        [JsonProperty(PropertyName = "alcoholtolerance")]
        public string AlcoholTolerance { get; set; }
        [JsonProperty(PropertyName = "productcode")]
        public string ProductCode { get; set; }
        [JsonProperty(PropertyName = "comment")]
        public string Comment { get; set; }
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }
        [JsonProperty(PropertyName = "supplier")]
        public DTO Supplier { get; set; }
    }
}