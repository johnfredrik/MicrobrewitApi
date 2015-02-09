using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Microbrewit.Model.DTOs
{
    public class UserPutDto
    {
        [Required]
        [Display(Name = "User name")]
        [JsonProperty(PropertyName = "username")]
        public string Username { get; set; }
        [StringLength(500, ErrorMessage = "The{0} must be at leaste {2} characters long", MinimumLength = 2)]
        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }
        [JsonProperty(PropertyName = "settings")]
        public string Settings { get; set; }
        [JsonProperty(PropertyName = "geoLocation")]
        public GeoLocationDto GeoLocation { get; set; }
        [JsonProperty(PropertyName = "headerImage")]
        public string HeaderImage { get; set; }
        [JsonProperty(PropertyName = "avatar")]
        public string Avatar { get; set; }
        [JsonProperty(PropertyName = "socials")]
        public Dictionary<string, string> Socials { get; set; }
    }
}
