using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Microbrewit.Model.DTOs
{
    public class RecipeHopDto
    {
        [JsonProperty(PropertyName = "hopId")]
        public int Id { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "aaValue")]
        public int AAValue { get; set; }
        [JsonProperty(PropertyName = "origin")]
        public string Origin { get; set; }
    }
}