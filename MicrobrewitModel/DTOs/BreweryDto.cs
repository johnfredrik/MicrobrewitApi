using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Microbrewit.Model.DTOs
{
    [ElasticType(Name = "brewery")]
    public class BreweryDto
    {
        [JsonProperty(PropertyName = "breweryId")]
        public int Id { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }
        [JsonProperty(PropertyName = "dataType")]
        public string DataType { get { return "brewery"; } }
        [JsonProperty(PropertyName = "members")]
        public IList<DTOUser> Members { get; set; }
        [JsonProperty(PropertyName = "beers")]
        public IList<DTO> Beers { get; set; }
    }
}
