using Nest;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Microbrewit.Model.DTOs
{
    [ElasticType(Name = "hop")]
    public class HopDto
    {
        [JsonProperty(PropertyName = "hopId")]
        public int Id { get; set; }
        [Required]
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "acids")]
        public AcidDto Acids { get; set; }
        [JsonProperty(PropertyName = "notes")]
        public string Notes { get; set; }
        [JsonProperty(PropertyName = "flavourDescription")]
        public String FlavourDescription { get; set; }
        [JsonProperty(PropertyName = "origin")]
        public DTO Origin { get; set; }
        [JsonProperty(PropertyName = "flavours")]
        public IList<string> Flavours { get; set; }
        [JsonProperty(PropertyName = "aromaWheel")]
        public IList<string> AromaWheel { get; set; }
        [JsonProperty(PropertyName = "aliases")]
        public IList<string> Aliases { get; set; }
        [JsonProperty(PropertyName = "purpose")]
        public string Purpose { get; set; }
        [JsonProperty(PropertyName = "substitutes")]
        public IList<DTO> Substituts { get; set; }
        [JsonProperty(PropertyName = "oils")]
        public OilDto Oils { get; set; }
        [JsonProperty(PropertyName = "dataType")]
        public string DataType { get { return "hop"; } }
        [Required]
        [JsonProperty(PropertyName = "custom")]
        public bool Custom { get; set; }
    }
}