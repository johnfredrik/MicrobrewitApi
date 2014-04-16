using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Microbrewit.Model.DTOs
{
    public class UserCompleteDto
    {
        public Links Links { get; set; }
        public IList<UserDto> Users { get; set; }

        public UserCompleteDto()
        {
            Links = new Links()
            {
                Href = "http://api.microbrew.it/breweries/:id",
                Type = "brewery"
            };
        }
    }
}