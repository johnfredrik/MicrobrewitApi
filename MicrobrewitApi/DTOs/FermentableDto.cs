using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Microbrewit.Api.DTOs
{
    public class FermentableDto
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }
        //[JsonProperty(PropertyName = "href")]
        //public string Href { get; set; }
        [JsonProperty(PropertyName = "maltster")]
        public DTO Maltster { get; set; }
        [JsonProperty(PropertyName = "fermentablename")]
        public string FermentableName { get; set; }
        [JsonProperty(PropertyName = "colour")]
        public int Colour { get; set; }
        [JsonProperty(PropertyName = "ppg")]
        public int PPG { get; set; }
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }
      }
}