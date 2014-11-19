using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace Microbrewit.Model.DTOs
{
    public class UserCompleteDto
    {
        private static readonly string apiPath = ConfigurationManager.AppSettings["api"];
        [JsonProperty(PropertyName = "links")]
        public Links Links { get; set; }
        [JsonProperty(PropertyName = "users")]
        public IList<UserDto> Users { get; set; }

        public UserCompleteDto()
        {
            Links = new Links()
            {
                Href = apiPath + "/breweries/:id",
                Type = "brewery"
            };
        }
    }
}