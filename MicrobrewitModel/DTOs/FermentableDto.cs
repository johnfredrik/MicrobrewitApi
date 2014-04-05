using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Microbrewit.Model.DTOs
{
    public class FermentableDto
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }
        //[JsonProperty(PropertyName = "href")]
        //public string Href { get; set; }
        [JsonProperty(PropertyName = "supplier")]
        public DTO Supplier { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "ebc")]
        public double EBC { get; set; }
        [JsonProperty(PropertyName = "ppg")]
        public int PPG { get; set; }
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }
      }
}