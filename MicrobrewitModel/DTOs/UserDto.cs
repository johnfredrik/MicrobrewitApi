using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace Microbrewit.Model.DTOs
{
    [ElasticType(Name = "user")]
    public class UserDto
    {
        [JsonProperty(PropertyName = "userId")]
        public string Id { get; set; }
        [JsonProperty(PropertyName = "username")]
        public string Username { get; set; }
        [JsonProperty(PropertyName = "gravatar")]
        public string Gravatar { get; set; }
        [JsonProperty(PropertyName = "breweries")]
        public IList<BreweryDto> Breweries { get; set; }
        [JsonProperty(PropertyName = "beers")]
        public IList<BeerDto> Beers { get; set; }
        [JsonProperty(PropertyName = "settings")]
        public string Settings { get; set; }
        [JsonProperty(PropertyName = "geoLocation")]
        public GeoLocationDto GeoLocation { get; set; }
        [JsonProperty(PropertyName = "emailConfirmed")]
        public bool EmailConfirmed { get; set; }
        [JsonProperty(PropertyName = "dataType")]
        public string DataType { get { return "user"; } }
        [JsonProperty(PropertyName = "headerImage")]
        public string HeaderImage { get; set; }
        [JsonProperty(PropertyName = "avatar")]
        public string Avatar { get; set; }
        [JsonProperty(PropertyName = "socials")]
        public Dictionary<string, string> Socials { get; set; }
    }
}