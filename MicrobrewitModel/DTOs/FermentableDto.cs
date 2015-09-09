using Nest;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Microbrewit.Model.DTOs
{
    [ElasticType(Name = "fermentable")]
    public class FermentableDto
    {
        [JsonProperty(PropertyName = "fermentableId")]
        public int Id { get; set; }
        [JsonProperty(PropertyName = "supplier")]
        public SupplierDto Supplier { get; set; }
        [Required]
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "lovibond")]
        public double Lovibond { get; set; }
        [JsonProperty(PropertyName = "ppg")]
        public int PPG { get; set; }
        [Required]
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }
        [JsonProperty(PropertyName = "dataType")]
        public string DataType { get { return "fermentable"; } }
        [Required]
        [JsonProperty(PropertyName = "custom")]
        public bool Custom { get; set; }
        [JsonProperty(PropertyName = "superFermentableId")]
        public int? SuperFermentableId { get; set; }
        [JsonProperty(PropertyName = "subFermentables")]
        public IList<DTO> SubFermentables { get; set; }

      }

}