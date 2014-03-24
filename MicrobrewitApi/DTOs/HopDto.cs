using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Microbrewit.Api.DTOs
{
    public class HopDto
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }
        [JsonProperty(PropertyName = "href")]
        public string Href { get { return "http://api.microbrew.it/hops/:id"; }}
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "aalow")]
        public int AALow { get; set; }
        [JsonProperty(PropertyName = "aahigh")]
        public int AAHigh { get; set; }
        [JsonProperty(PropertyName = "origin")]
        public string Origin { get; set; }
        [JsonProperty(PropertyName = "flavours")]
        public IList<string> Flavours { get; set; }
        [JsonProperty(PropertyName = "substitutions")]
        public IList<String> Substitutions { get; set; }
        [JsonProperty(PropertyName = "links")]
        public HopLinks Links { get; set; }

        
    }
}