using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Microbrewit.Model.DTOs
{
    public class UserCompleteDto
    {
        private static readonly string apiPath = ConfigurationManager.AppSettings["api"];
        public Links Links { get; set; }
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