using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nest;
using Newtonsoft.Json;

namespace Microbrewit.Model.DTOs
{
    [ElasticType(Name = "breweryMember")]
    public class BreweryMemberDto
    {
        [Required]
        [JsonProperty(PropertyName = "username")]
        public string Username { get; set; }
        [JsonProperty(PropertyName = "gravatar")]
        public string Gravatar { get; set; }
        [JsonProperty(PropertyName = "role")]
        public string Role { get; set; }
        [JsonProperty(PropertyName = "dataType")]
        public string DataType { get { return "breweryMember"; } }

    }
}
