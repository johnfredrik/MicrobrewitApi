using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Microbrewit.Model.DTOs
{
    [ElasticType(Name = "brewery")]
    public class BreweryDto
    {
        [JsonProperty(PropertyName = "breweryId")]
        public int Id { get; set; }
        [Required]
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }
        [JsonProperty(PropertyName = "dataType")]
        public string DataType { get { return "brewery"; } }
        [JsonProperty(PropertyName = "members")]
        public IEnumerable<BreweryMemberDto> Members { get; set; }
        [JsonProperty(PropertyName = "beers")]
        public IEnumerable<DTO> Beers { get; set; }
        [JsonProperty(PropertyName = "geoLocation")]
        public GeoLocationDto GeoLocation { get; set; }
    }
}
