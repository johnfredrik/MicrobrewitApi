using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Microbrewit.Api.DTOs
{
    
    public class Links
    {
         [JsonProperty(PropertyName = "href")]
        public string Href { get; set; }
         [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }
    }
}